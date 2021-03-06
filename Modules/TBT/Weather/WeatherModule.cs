using UnityEngine;

namespace XiheFramework {
    public class WeatherModule : GameModule {
        public Light sun;

        public Color dayColor; //12am
        public Color nightColor; //12pm

        [Range(1, 12)]
        public int month = 0;

        [Range(1, 30)]
        public int day = 0;

        [Range(1, 24)]
        public int hour = 0;

        [Range(1, 60)]
        public int minute = 0;

        [Range(1, 60)]
        public int second = 0;

        private Vector3 m_TargetEuler;
        private Transform m_CachedTransform;

        public void SetDate(int m, int d) {
            this.month = m;
            this.day = d;

            var yt = (month * 30 + day) / 360f;
            Game.Event.Invoke("OnSetDate", this, yt);
        }

        public void SetDate(float t) {
            t %= 1f;

            var total = t * 360;
            month = (int) (total / 30f);
            day = (int) ((total - month * 30f) / 60f);

            Game.Event.Invoke("OnSetDate", this, t);
        }

        public void SetTime(int h, int m, int s) {
            this.hour = h;
            this.minute = m;
            this.second = s;

            var dt = (hour * 3600 + minute * 60 + second) / 86400f;

            Game.Event.Invoke("OnSetTime", this, dt);
        }

        public void SetTime(float t) {
            t %= 1f;
            var total = t * 86400;

            hour = (int) (total / 24f);
            minute = (int) ((total - hour * 3600f) / 60f);
            second = (int) ((total - hour * 3600f - minute * 60f) / 60f);


            Game.Event.Invoke("OnSetTime", this, t);
        }

        public override void Setup() {
            base.Setup();

            m_CachedTransform = sun.transform;
        }

        public override void Update() {
            UpdateTargetEuler();
            // m_CachedTransform.localRotation = Quaternion.Euler(m_TargetEuler.x, 0f, 0f);
            // m_CachedTransform.rotation = Quaternion.Euler(m_CachedTransform.rotation.x, m_TargetEuler.y, 0f);
            m_CachedTransform.rotation = Quaternion.Euler(m_TargetEuler);

            UpdateSunColor();
        }

        private void UpdateSunColor() {
            var dt = (hour * 3600 + minute * 60 + second) / 86400f;
            if (dt < 0.5f) {
                //12pm - 12am
                sun.color = Color.Lerp(nightColor, dayColor, dt * 2f);
            }
            else {
                sun.color = Color.Lerp(dayColor, nightColor, dt * 2f - 1f);
            }
        }

        public override void ShutDown(ShutDownType shutDownType) {
        }

        void UpdateTargetEuler() {
            var yt = (month * 30 + day) / 360f;
            var dt = (hour * 3600 + minute * 60 + second) / 86400f;

            m_TargetEuler.x = (dt * 360f - 90f) % 360f;
            m_TargetEuler.y = (yt * 360f) % 360f;
        }
    }
}