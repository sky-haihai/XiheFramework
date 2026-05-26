using UnityEngine;

namespace XiheFramework.Runtime.Utility {
    public static class RigidBodyHelper {
        //retrieved from https://answers.unity.com/questions/819273/force-to-velocity-scaling.html?childToView=819474#answer-819474
        public static float GetFinalVelocity(float aVelocityChange, float aDrag) {
            return aVelocityChange * (1 / Mathf.Clamp01(aDrag * Time.fixedDeltaTime) - 1);
        }

        public static float GetFinalVelocityFromAcceleration(float aAcceleration, float aDrag) {
            return GetFinalVelocity(aAcceleration * Time.fixedDeltaTime, aDrag);
        }

        public static float GetDrag(float aVelocityChange, float aFinalVelocity) {
            return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
        }

        public static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity) {
            return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
        }

        public static float GetRequiredVelocityChange(float aFinalSpeed, float aDrag) {
            var m = Mathf.Clamp01(aDrag * Time.fixedDeltaTime);
            return aFinalSpeed * m / (1 - m);
        }

        public static float GetRequiredAcceleraton(float aFinalSpeed, float aDrag) {
            return GetRequiredVelocityChange(aFinalSpeed, aDrag) / Time.fixedDeltaTime;
        }
    }
}