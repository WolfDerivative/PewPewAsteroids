using System.Collections.Generic;
using UnityEngine;


public class SpaceshipControlls: MonoBehaviour {

    public float MaxVelocity = 30f;
    [Tooltip("How fast the spinner accelerates to the max velocity.")]
    public float Acceleration = 5f;
    [Tooltip("How fast spaceship slows down when opposit direction is pressed.")]
    public float BrakeForce = 600;
    public float RotationSensitivity = 8;
    [Tooltip("velocity = BoostMultiplier * Aceeleration.")]
    public float BoostMultiplier = 2.0f;
    public float MaxBoostVelocity = 300;
    public ParticleSystem effect_accl;
    public ParticleSystem effect_boost;

    public bool     isUnlocked = false;
    public int      pointsToUnlock = 200;
    public bool     IsAutoshoot = true;

    public Vector2 Velocity { get { return (_rigidBody != null) ? _rigidBody.velocity : Vector2.zero; } }
    public DamagableSpaceship DamageDealer { get { return _damagable; } }
    public Rigidbody2D RB { get { return _rigidBody; } }
    public bool IsCanMove { get { return bIsCanMove; } }
    public bool IsBoostOn {
        get {
            return Input.GetAxis("Boost") != 0 || _keyboard.IsKeyDown("Boost") != 0;
        }
    }

    protected CircleCollider2D _circleCollider;
    protected Weapon            _weapon;
    protected UserInputManager  _keyboard;

    private Animator            _animator;
    private Rigidbody2D         _rigidBody;
    private DamagableSpaceship  _damagable;
    private bool                bIsShooting, bIsBtnShooting;
    private bool                bIsCanMove;


    /* ------------------------------------------------------------------------------ */


    // Use this for initialization
    void Start() {
        _circleCollider = GetComponent<CircleCollider2D>();
        _damagable = GetComponent<DamagableSpaceship>();
        _weapon = GetComponentInChildren<Weapon>();
        _keyboard = UserInputManager.Instance;

        _animator = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        Reset();
    }//Start


    public void Update() {
        if (GameManager.Instance.IsGameOver) return;

        if (Time.timeScale == 0) return;

        if (_damagable.HealthStatus <= 0) GameManager.Instance.GameOver();

        if (_keyboard.IsKeyDown("ResetAim") != 0)
            _weapon.transform.rotation = new Quaternion(0, 0, 0, 0);
        Aim();

        if (IsAutoshoot || bIsShooting || bIsBtnShooting) {
            _weapon.PullTrigger(); //Autoshoot
        } else {
            _weapon.ReleaseTrigger();
        }
    }//Update


    public void FixedUpdate() {
        float deltaMovement = Mathf.Abs(Velocity.x);    //deltamovement must be positive at all times. Otherwise, -x speed less than +x

        float verticalAxis = Input.GetAxis("Vertical") * -1;
        verticalAxis = (verticalAxis == 0) ? _keyboard.IsKeyDown("Vertical") : verticalAxis;

        float horizontalAxis = Input.GetAxis("Horizontal");
        horizontalAxis = (horizontalAxis == 0) ? _keyboard.IsKeyDown("Horizontal") : horizontalAxis;

        float acceleration = OnBoost(Acceleration, verticalAxis != 0);  //Modified acceleration if Boost button is pressed.

        if (verticalAxis != 0)
            deltaMovement += acceleration * Time.deltaTime * Mathf.Sign(verticalAxis);

        Move(ref deltaMovement);

        if (verticalAxis != 0) effect_accl.Play();
        else effect_accl.Stop();

        Turn(horizontalAxis);

        _animator.SetBool("IsMoving", verticalAxis != 0);
        if (Mathf.Abs(Velocity.magnitude) <= 1.0 && verticalAxis == 0)
            _rigidBody.velocity = Vector2.zero;

    }//FixUpdate

    /// <summary>
    ///  Check if Boost button is pressed and return acceleration*boost value, where
    /// boost = 1 when no button pressed, boost = BoostMultiplier otherwise.
    /// </summary>
    /// <param name="acceleration">Acceleration value to be added to the velocity.</param>
    /// <returns></returns>
    protected float OnBoost(float acceleration, bool isVerticalAxis) {
        float boost = (IsBoostOn) ? BoostMultiplier : 1;
        acceleration *= boost;
        if (boost != 1 && isVerticalAxis) { 
            if (effect_boost != null)
                effect_boost.Play();
        } else{
            if (effect_boost != null)
                effect_boost.Stop();
        }
        return acceleration;
    }//PlayEffects


    /// <summary>
    ///  Move spaceship in the direction and speed of deltaMovement vector using
    /// RigidBody's relative force.
    /// </summary>
    /// <param name="deltaMovement"> Direction and speed of movement. </param>
    protected void Move(ref float deltaMovement) {
        if (!IsCanMove)
            return;

        float maxVelocity = (IsBoostOn) ? MaxBoostVelocity : MaxVelocity;

        if (Mathf.Abs(Velocity.magnitude) > maxVelocity) {
            deltaMovement = (MaxVelocity) * Mathf.Sign(deltaMovement);
            return;
        }
        _rigidBody.AddRelativeForce(deltaMovement * Vector2.up);
    }//Move


    /// <summary>
    ///   Rotate spaceship by deltaRotation amount.
    /// </summary>
    /// <param name="deltaRotation"> Horizontal and Vertical input that makes up rotation. </param>
    protected void Turn(float deltaRotation) {
        _rigidBody.AddTorque(deltaRotation * -RotationSensitivity);
        int turningSign = (deltaRotation == 0) ? 0 : Mathf.FloorToInt( Mathf.Sign(deltaRotation));
        _animator.SetInteger("Turning", turningSign);
    }//Aim


    /// <summary>
    ///  Aim Turret at direction of joystick stick.
    /// Ref: http://answers.unity3d.com/questions/660768/how-to-shoot-using-the-right-mobile-joystick.html
    /// </summary>
    public void Aim() {
        if (_keyboard.IsKeyDown("Fire") != 0)
            bIsBtnShooting = true;
        if (_keyboard.IsKeyDown("Fire") == 0)
            bIsBtnShooting = false;

        if (bIsBtnShooting)
            return;

        var x = Input.GetAxis("HorizontalAim");
        var y = Input.GetAxis("VerticalAim");
        if (x != 0.0 || y != 0.0) {
            var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            var deltaRotation = Quaternion.AngleAxis(90 + angle, -Vector3.forward);
            bIsShooting = true;
            _weapon.gameObject.transform.rotation = deltaRotation;
        } else {
            bIsShooting = false;
        }
    }//Aim


    public void Reset() {
        SetIsCanMove(true);
        _rigidBody.velocity= Vector3.zero;
        _damagable.Reset();
    }//Reset

    public void SetIsCanMove(bool state) { bIsCanMove = state; }
    public void SetAnimationSpeed(float speed) { _animator.speed = speed; }
    public CircleCollider2D GetCollider() { return _circleCollider; }


    public void OnDisable() {
    }//OnDisable


    public void OnTriggerEnter2D(Collider2D collision) {
    }//OnTriggerEnter2D


}//class
