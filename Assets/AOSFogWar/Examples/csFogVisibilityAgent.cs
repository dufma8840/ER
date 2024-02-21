using UnityEngine;                  // Monobehaviour
using System.Collections.Generic;   // List
using System.Linq;                  // ToList



namespace FischlWorks_FogWar
{
    public class csFogVisibilityAgent : MonoBehaviour
    {
        [SerializeField]
        private csFogWar fogWar = null;

        [SerializeField]
        public bool visibility = false;

        [SerializeField]
        [Range(0, 2)]
        private int additionalRadius = 0;

        private List<MeshRenderer> meshRenderers = null;
        private List<SkinnedMeshRenderer> skinnedMeshRenderers = null;
        static public csFogVisibilityAgent Instance;

        public  static csFogVisibilityAgent Getfog()
        {
            return Instance;
        }



        private void Start()
        {
            // This part is meant to be modified following the project's scene structure later...
            try
            {
                fogWar = GameObject.Find("FogWar").GetComponent<csFogWar>();
            }
            catch
            {
               
            }

            meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        }



        private void Update()
        {
            if (fogWar == null || fogWar.CheckWorldGridRange(transform.position) == false)
            {
                return;
            }

            visibility = fogWar.CheckVisibility(transform.position, additionalRadius);

            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = visibility;
            }

            foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
            {
                renderer.enabled = visibility;
            }
        }
    }
}
     