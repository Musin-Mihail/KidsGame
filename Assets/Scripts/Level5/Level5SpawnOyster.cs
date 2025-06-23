using System.Collections;
using UnityEngine;

namespace Level5
{
    public class Level5SpawnOyster : MonoBehaviour
    {
        public SpriteRenderer _SpriteRenderer;

        private void Start()
        {
            _SpriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(StartStage());
        }

        public IEnumerator ChangeStage()
        {
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
            yield return new WaitForSeconds(0.4f);
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[0];
            yield return new WaitForSeconds(0.4f);
            if (Level5Global.NewColarFigures.Count > 0)
            {
                _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
                yield return new WaitForSeconds(0.4f);
                _SpriteRenderer.sprite = Level5Global.NewStageOyster[2];
                var newVector3 = new Vector3(transform.position.x, transform.position.y, -0.25f);
                var instFirure = Instantiate(Level5Global.NewColarFigures[0], newVector3, Level5Global.NewColarFigures[0].transform.rotation, gameObject.transform);
                Level5Global.ReadyFigures.Add(instFirure);
                Level5Global.NewColarFigures.RemoveAt(0);
            }
        }

        private IEnumerator StartStage()
        {
            yield return new WaitForSeconds(0.4f);
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
            yield return new WaitForSeconds(0.4f);
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[2];
            var newVector3 = new Vector3(transform.position.x, transform.position.y, -0.25f);
            var instFirure = Instantiate(Level5Global.NewColarFigures[0], newVector3, Level5Global.NewColarFigures[0].transform.rotation, gameObject.transform);
            Level5Global.ReadyFigures.Add(instFirure);
            Level5Global.NewColarFigures.RemoveAt(0);
        }
    }
}