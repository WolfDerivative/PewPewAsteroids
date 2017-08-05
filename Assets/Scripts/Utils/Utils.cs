using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils {
    public static class Utils {

        /// <summary>
        ///  Conver box collider bounds to world space.
        /// Taken from: http://answers.unity3d.com/questions/605550/how-to-convert-a-boxcollider2d-bounds-to-world-spa.html by vargonian.
        /// </summary>
        /// <param name="collider"> box collider component. </param>
        /// <returns></returns>
        public static Rect BountsToWorld(ref BoxCollider2D collider) {
            float worldRight = collider.transform.TransformPoint(collider.offset + new Vector2(collider.size.x * 0.5f, 0)).x;
            float worldLeft = collider.transform.TransformPoint(collider.offset - new Vector2(collider.size.x * 0.5f, 0)).x;

            float worldTop = collider.transform.TransformPoint(collider.offset + new Vector2(0, collider.size.y * 0.5f)).y;
            float worldBottom = collider.transform.TransformPoint(collider.offset - new Vector2(0, collider.size.y * 0.5f)).y;
            return new Rect(worldTop,
                             worldRight,
                             worldBottom,
                             worldLeft
                            );
        }//BountsToWorld


        /// <summary>
        ///  Returns border coordinates of the box collider bounds. Use xMin, xMax, yMin, yMax.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns float>float[]{ top, right, bottom, left }</returns>
        public static Rect GetCorners(Bounds bounds) {
            Rect result = new Rect();
            result.xMin = bounds.center.x - bounds.extents.x;
            result.xMax = bounds.center.x + bounds.extents.x;

            result.yMax = bounds.center.y + bounds.extents.y;
            result.yMin = bounds.center.y - bounds.extents.y;
            return result;
        }//GetCorners


        /// <summary>
        ///  Transform Camera bounds (width, height) to world location.
        ///  Taken from: http://answers.unity3d.com/questions/501893/calculating-2d-camera-bounds.html by GeekyMonkey.
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Bounds OrthographicBounds(this Camera camera) {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            return new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        }//OrthographicBounds



        public static bool[] IsWithinBounds(Bounds checkWithin, Vector3 target) {
            var inHorizontal = target.x > checkWithin.min.x && target.x < checkWithin.max.x;
            var inVertical = target.y > checkWithin.min.y && target.y < checkWithin.max.y;
            return new bool[] { inHorizontal, inVertical };
        }


        /// <summary>
        ///  Unity Editor warning message only about not finding the game object
        /// on the scene.
        /// </summary>
        /// <param name="goName">Name of the game obbject that was not found.</param>
        public static void WarningGONotFound(string goName) {
        #if UNITY_EDITOR
            Debug.LogWarning("Could find Spawner with the name '" + goName + "'!");
        #endif
        }


        public static void WarningMessage(string msg) {
            #if UNITY_EDITOR
                Debug.LogWarning(msg);
#endif
        }//WarningMessage


        /// <summary>
        ///  Helps debugging joystick buttons names...
        /// </summary>
        public static void PrintAnyKeyDown() {
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }
        }//PrintAnyKeyDown


    }//Utils

}//namespace
