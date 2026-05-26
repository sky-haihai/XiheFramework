using System.Collections.Generic;
using UnityEngine;

namespace XiheFramework.Runtime.Entity.BuiltinEntity {
    public sealed class ParticleEntity : GameEntityBase {
        public ParticleSystem particle;
        
        private ParticleSystem.MainModule m_MainModule;
        private List<ParticleSystem.MainModule> m_SubMainModules = new List<ParticleSystem.MainModule>();
        private ParticleSystemRenderer m_ParticleSystemRenderer;
        private bool m_Loop;

        public override string GroupName => "ParticleEntity";

        #region Public Methods

        public void Play(bool loop) {
            m_MainModule.loop = loop;
            m_SubMainModules.ForEach(subMainModule => subMainModule.loop = loop);
            particle.Play(true);
        }

        public void SetMaterial(Material material) {
            m_ParticleSystemRenderer.material = material;
        }

        #endregion

        #region Life Cycle

        protected override void OnInitCallback() {
            if (!particle) {
                particle = GetComponent<ParticleSystem>();
            }

            InitMainModules();
        }

        protected override void OnSetGlobalTimeScale(object sender, object e) {
            base.OnSetGlobalTimeScale(sender, e);

            m_MainModule.simulationSpeed = TimeScale;
            for (var i = 0; i < m_SubMainModules.Count; i++) {
                var subMainModule = m_SubMainModules[i];
                subMainModule.simulationSpeed = TimeScale;
            }
        }

        private void OnParticleSystemStopped() {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if (!particle) {
                particle = GetComponent<ParticleSystem>();
            }
        }
#endif

        #endregion

        #region Private Methods

        private void InitMainModules() {
            m_ParticleSystemRenderer = particle.GetComponent<ParticleSystemRenderer>();
            m_MainModule = particle.main;
            m_MainModule.playOnAwake = false;
            m_MainModule.stopAction = ParticleSystemStopAction.Callback;
            m_MainModule.simulationSpeed = TimeScale;

            var childParticles = particle.GetComponentsInChildren<ParticleSystem>();
            for (var i = 1; i < childParticles.Length; i++) {
                var subParticle = childParticles[i];
                var subMainModule = subParticle.main;
                subMainModule.simulationSpeed = TimeScale;
                m_SubMainModules.Add(subMainModule);
            }
        }

        #endregion
    }
}