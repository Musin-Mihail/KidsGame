using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5SpawnOyster : MonoBehaviour
{
    public GameObject ReadyFigure;
    public SpriteRenderer _SpriteRenderer;
    int Stage = 1;
    void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StartStage());
    }

    void Update()
    {
        if(ReadyFigure == null && Stage == 3)
        {
            StartCoroutine(ChangeStage());
        }
    }
    IEnumerator ChangeStage()
    {
        Stage = 2;
        _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
        yield return new WaitForSeconds(0.5f);
        Stage = 1;
        _SpriteRenderer.sprite = Level5Global.NewStageOyster[0];
        yield return new WaitForSeconds(1);
        if(Level5Global.NewColarFigures.Count > 0)
        {
            Stage = 2;
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
            yield return new WaitForSeconds(0.5f);
            _SpriteRenderer.sprite = Level5Global.NewStageOyster[2];
            Vector3 newVector3 = new Vector3(transform.position.x, transform.position.y,-0.25f);
            var instFirure = Instantiate(Level5Global.NewColarFigures[0], newVector3, Level5Global.NewColarFigures[0].transform.rotation);
            // instFirure.name = Level5Global.NewColarFigures[0].name;
            Level5Global.ReadyFigures.Add(instFirure);
            ReadyFigure = instFirure;
            Level5Global.NewColarFigures.RemoveAt(0);
            Stage = 3;
        }
    }
    IEnumerator StartStage()
    {
        yield return new WaitForSeconds(1);
        Stage = 2;
        _SpriteRenderer.sprite = Level5Global.NewStageOyster[1];
        yield return new WaitForSeconds(0.5f);
        _SpriteRenderer.sprite = Level5Global.NewStageOyster[2];
        Vector3 newVector3 = new Vector3(transform.position.x, transform.position.y,-0.25f);
        var instFirure = Instantiate(Level5Global.NewColarFigures[0], newVector3, Level5Global.NewColarFigures[0].transform.rotation);
        // instFirure.name = Level5Global.NewColarFigures[0].name;
        ReadyFigure = instFirure;
        Level5Global.ReadyFigures.Add(instFirure);
        Level5Global.NewColarFigures.RemoveAt(0);
        Stage = 3;
    }
}