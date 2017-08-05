using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EMeteor : Enemy {

    [Tooltip("X = min velocity; Y = max velocity;")]
    public Vector2 MaxVelocity;
    public SpawnChances Chances { get { return _chances; } }
    [Tooltip("Explosion effects to be used(randomly if more than one).")]
    public GameObject[] ExplosionEffectPrefab;
    [Tooltip("Name of the pool to get smaller meteors on destroy of this one.")]
    public string BreakablePiecesPoolName = "Meteor_med_pool";
    [Tooltip("Min and Max number of piecies to be spawned on destroy.")]
    public Vector2 MaxDebriesPiecies = new Vector2(1, 2);
    public OrbitingProps Orbit;

    public Vector2 Velocity { get { return velocity; } }
    public bool IsDisabledThisFrame { get { return bIsDisabledThisFrame; } }

    protected CircleCollider2D _circleCollider;
    protected MeteorPool breakablePieces;
    //protected DamagableMeteor _damagable;
    protected SpawnChances _chances;
    protected Vector2 velocity;
    protected Vector3 moveTowards;

    private bool isLaunch; //whether or not meteor has been launch towards the target. False - do nothing.
    private bool bIsDisabledThisFrame;      //object was disabled on This frame (cant enable on the same frame)
    private bool bResetDisabledThisFrame;   //flag next frame to reset bIsDisabledThisFrame flag.


    public override void Start() {
        base.Start();
        bIsDisabledThisFrame = false;
        _circleCollider = GetComponent<CircleCollider2D>();
        _chances = (_chances == null) ? GetComponent<SpawnChances>() : _chances;
        var poolObj = GameObject.Find(BreakablePiecesPoolName);
        if(poolObj != null)
            breakablePieces = poolObj.GetComponent<MeteorPool>();
        velocity = MaxVelocity;
    }//Start


    public void Update() {
        if (GameManager.Instance.SpaceshipPrefab == null)
            return;

        if (_damagable.IsDead) {
            this.gameObject.SetActive(false);
        }//if

        if (this.transform.position == GameManager.Instance.SpaceshipPrefab.transform.position)  //PARANOIA?? should never happened...
            this.gameObject.SetActive(false);

        if (bResetDisabledThisFrame) {
            bResetDisabledThisFrame = false;
            bIsDisabledThisFrame = false;
        }
        if (IsDisabledThisFrame) {
            bResetDisabledThisFrame = true;
        }
    }//Update


    public void FixedUpdate() {
        if (!isLaunch)
            return;

        if (Orbit.ToOrbitAround) {
            transform.RotateAround(Orbit.Transform.position, Vector3.forward, Velocity.x * Time.deltaTime);
            Vector3 orbitDesiredPosition = (transform.position - Orbit.Transform.position).normalized * Orbit.Radius + Orbit.Transform.position;
            transform.position = Vector3.Slerp(transform.position, orbitDesiredPosition, Time.deltaTime * 10);
        } else {
            // velocity is set random on Enabled between MaxVelocity.x / 2 and MaxVelocity.x
            Move(this.transform.right * velocity.x * 10 * Time.deltaTime);
        }
    }//FixedUpdate


    /// <summary>
    ///  Spawn smaller object at the location of this object.
    ///  Set random direction movement and velocity for the
    ///  spawned debries.
    /// </summary>
    /// <param name="debries">(optional) number of debries to spawn. 
    /// If not passed, a random number between MexDebries X and Y will be chosen. </param>
    public void BreakIntoPieces(int debries=-1) {
        if (breakablePieces == null)
            return;

        if (debries == -1)
            debries = Random.Range(Mathf.RoundToInt(MaxDebriesPiecies.x), Mathf.RoundToInt(MaxDebriesPiecies.y + 1));

        for (int i = 0; i < debries; i++) {
            var smallerPiece = breakablePieces.GetInactive();           //Gettign debries object from the pool
            smallerPiece.transform.position = this.transform.position;  //Set its position to This object
            var cmp = breakablePieces.GetMeteorCmp(smallerPiece);       //Component is used to set Velocity

            smallerPiece.SetActive(true);

            var rnd = Random.Range(0f, 360f);  //FIXME
            smallerPiece.transform.eulerAngles = new Vector3(0, 0, rnd);

            cmp.SetRandomVelocity();
        }//for
    }//BreakIntoPieces


    /// <summary>
    ///     Return a random point on the Spaceship collider to move towards to.
    /// </summary>
    /// <returns> Vector3 position to move towards to.</returns>
    public Vector3 RandomPointOnTarget() {
        if (GameManager.Instance == null)
            return Vector3.zero;

        var spaceshipCmp = GameManager.Instance.GetSpaceshipCmp();
        if(spaceshipCmp == null) {
            Debug.LogWarning("SpaceshipCmp is null!!");
            return Vector3.zero;
        }
        CircleCollider2D targetCircle = spaceshipCmp.GetCollider();
        if(targetCircle == null) {
            #if UNITY_EDITOR
                Debug.LogWarning("Spaceship's collider not set? Spaceship name: " + GameManager.Instance.GetActiveSpaceship().name);
            #endif
            return Vector3.zero;
        }
        var targetBounds = targetCircle.bounds;
        float up = targetBounds.center.x + targetBounds.extents.x;
        float down = targetBounds.center.x - targetBounds.extents.x;
        return new Vector3(targetBounds.center.x, Random.Range(down, up), 0);
    }//RandomPointOnTarget


    public void SetDirection(Vector3 target) {
        moveTowards = target;
        Vector3 relativePos = moveTowards - transform.position;

        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }//SetDirection


    public void SetVelocity(Vector2 vel) { velocity = vel; }
    public void SetRandomVelocity() {
        velocity = new Vector2(Random.Range(MaxVelocity.x, MaxVelocity.y), 0);
    }//SetRandomVelocity


    /// <summary>
    ///  Check if enemy is within the screen/world bounds.
    /// If not, it probably has not been destroyed yet and is not
    /// visible to player. Therefore - destroy it.
    /// </summary>
    public void SelfDestroy() {
        if (WorldBounds.Instance == null)
            return;         //No worldbounds instance found.
        if (_circleCollider.IsTouching(WorldBounds.Instance.Collider)) 
            return;         //object still within the bounds
        this.gameObject.SetActive(false);
    }//SelfDestroy


    public override void OnEnable() {
        base.OnEnable();
        if (_damagable == null)
            Start();

        _damagable.Reset();
        
        SetVelocity(new Vector2(Random.Range(MaxVelocity.x, MaxVelocity.y), 0.0f));

        isLaunch = true;

        InvokeRepeating("SelfDestroy", LifeSpan, LifeSpan / 2);
    }//OnEnable


    public override void OnDisable() {
        base.OnDisable();
        bIsDisabledThisFrame = true;

        if (_damagable.IsDead) {
            int explosionIndex = Random.Range(0, ExplosionEffectPrefab.Length);
            Instantiate(ExplosionEffectPrefab[explosionIndex], this.transform.position, this.transform.rotation);

            BreakIntoPieces();
        }

        isLaunch = false;
    }//OnDisable

}//class


[System.Serializable]
public class OrbitingProps {

    public GameObject ToOrbitAround;
    public float Radius = 20;

    public Transform Transform { get { return (ToOrbitAround != null) ? ToOrbitAround.transform : null; } }

}//class