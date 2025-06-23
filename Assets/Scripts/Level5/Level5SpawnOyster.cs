using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5SpawnOyster : MonoBehaviour
{
    public GameObject ReadyFigure;
    public SpriteRenderer _SpriteRenderer;
    void Start()
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
        if(Level5Global.NewColarFigures.Count > 0)
        {
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
            yield return new WaitForSeconds(0.4f);
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[2];
            Vector3 newVector3 = new Vector3(transform.position.x, transform.position.y,-0.25f);
            var instFirure = Instantiate(Level5Global.NewColarFigures[0], newVector3, Level5Global.NewColarFigures[0].transform.rotation, gameObject.transform);
            Level5Global.ReadyFigures.Add(instFirure);
            ReadyFigure = instFirure;
            Level5Global.NewColarFigures.RemoveAt(0);
        }
    }
    IEnumerator StartStage()
    {
        yield return new WaitForSeconds(0.4f);
        _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
        yield return new WaitForSeconds(0.4f);
        _SpriteRenderer.sprite = Level5Global.NewStageOyster[2];
        Vector3 newVector3 = new Vector3(transform.position.x, transform.position.y,-0.25f);
        var instFirure = Instantiate(Level5Global.NewColarFigures[0], newVector3, Level5Global.NewColarFigures[0].transform.rotation, gameObject.transform);
        ReadyFigure = instFirure;
        Level5Global.ReadyFigures.Add(instFirure);
        Level5Global.NewColarFigures.RemoveAt(0);
    }
}