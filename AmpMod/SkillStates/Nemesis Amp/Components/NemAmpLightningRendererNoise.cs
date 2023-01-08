using UnityEngine;
using RoR2;
using RoR2.Orbs;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpLightningRendererNoise : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        [Header("Max Z, e.g. max width of the lightning fluctuation")]
        private float maxZ = 8f;

        [Header("Number of lightning bolt segments; increase to make more chaotic")]
        private int numSegments = 12;

        private Color color = Color.white;

        [Header("Range of individual lightning bolt fluctuations")]
        private float posRange = 0.15f;
        private float radius = 1f;
        private Vector2 midpoint;

        void Update ()
        {
            lineRenderer= GetComponent<LineRenderer>();
            for (int i = 0; i < numSegments-1; i++)
            {
                float z = ((float)i) * (maxZ)/ (float)(numSegments-1);
                lineRenderer.SetPosition(i, new Vector3(Random.Range(-posRange, posRange), Random.Range(-posRange, posRange), z));
             }

            lineRenderer.SetPosition(0, new Vector3(0f, 0f, 0f));
            lineRenderer.SetPosition(numSegments - 1, new Vector3(0f, 0f, 8f));
        } 

        /* void Update () {
            color.a = -10f * Time.deltaTime;
            lineRenderer.SetColors(color, color);
            if (color.a <= 0f)
            {
                Destroy(this.gameObject);
            }
        } */
    }
}
