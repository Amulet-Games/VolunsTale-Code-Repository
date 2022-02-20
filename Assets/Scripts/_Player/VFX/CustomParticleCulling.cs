using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CustomParticleCulling : MonoBehaviour
    {
        [Header("Configuration.")]
        public float cullingRadius = 10;
        public bool drawDebugSphere;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isCullingInited;

        [Header("Drag and Drops.")]
        [SerializeField] ParticleSystem m_Particle;
        [SerializeField] Light m_ParticleLight;

        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] Camera _statesCamera;
        [ReadOnlyInspector, SerializeField] Renderer m_ParticleRenderer;
        [ReadOnlyInspector, SerializeField] SessionManager _sessionManager;
        CullingGroup m_CullingGroup;

        void Start()
        {
            _sessionManager = SessionManager.singleton;
        }

        void Update()
        {
            if (!isCullingInited)
            {
                if (_sessionManager.isCurrentAsyncOperationFinished)
                {
                    Init();
                    isCullingInited = true;
                }
            }
        }

        void OnEnable()
        {
            if (m_CullingGroup != null)
                m_CullingGroup.enabled = true;
        }

        void OnDisable()
        {
            m_CullingGroup.enabled = false;

            m_Particle.Play(true);
            SetRendererStatus(true);
        }

        void OnDestroy()
        {
            if (m_CullingGroup != null)
                m_CullingGroup.Dispose();
        }

        IEnumerator CreateCullingGroup()
        {
            yield return new WaitForEndOfFrame();

            if (m_CullingGroup == null)
            {
                //Debug.Log("Set Culling Group");
                m_CullingGroup = new CullingGroup();
                m_CullingGroup.targetCamera = _statesCamera;
                m_CullingGroup.SetBoundingSpheres(new[] { new BoundingSphere(transform.position, cullingRadius) });
                m_CullingGroup.SetBoundingSphereCount(1);
                m_CullingGroup.onStateChanged += OnStateChanged;

                // We need to start in a culled state
                Cull(m_CullingGroup.IsVisible(0));
            }
        }

        // This will be called after each sphere has changed state, e.g. culling is complete
        void OnStateChanged(CullingGroupEvent sphere)
        {
            /*
            if (sphere.hasBecomeVisible)
                Debug.LogFormat("Sphere {0} has become visible!", sphere.index);

            if (sphere.hasBecomeInvisible)
                Debug.LogFormat("Sphere {0} has become invisible!", sphere.index);
            */

            Cull(sphere.isVisible);
        }

        void Cull(bool visible)
        {
            if (visible)
            {
                // We could simulate forward a little here to hide that the system was not updated off-screen.
                m_Particle.Play(true);
                SetRendererStatus(true);
                if (m_ParticleLight != null)
                    m_ParticleLight.enabled = true;
            }
            else
            {
                m_Particle.Pause(true);
                SetRendererStatus(false);
                if (m_ParticleLight != null)
                    m_ParticleLight.enabled = false;
            }
        }

        void SetRendererStatus(bool enable)
        {
            // We also need to disable the renderer to prevent drawing the particles, such as when occlusion happens
            m_ParticleRenderer.enabled = enable;
        }

        void Init()
        {
            m_ParticleRenderer = m_Particle.GetComponent<Renderer>();
            _statesCamera = _sessionManager._camHandler.mainCamera;

            StartCoroutine(CreateCullingGroup());
        }

        void OnDrawGizmos()
        {
            if (drawDebugSphere)
            {
                if (enabled)
                {
                    // Draw gizmos to show the culling sphere.
                    Color col = Color.yellow;
                    if (m_CullingGroup != null && !m_CullingGroup.IsVisible(0))
                        col = Color.gray;

                    Gizmos.color = col;
                    Gizmos.DrawWireSphere(transform.position, cullingRadius);
                }
            }
        }
    }
}