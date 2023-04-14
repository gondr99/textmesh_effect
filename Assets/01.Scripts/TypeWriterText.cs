using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterText : MonoBehaviour
{
    [SerializeField]
    private float _oneCharacterTime = 0.2f;
    [SerializeField]
    private Color _startColor, _endColor;

    private bool _isType = false;

    private TMP_Text _tmpText;
    
    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && _isType == false)
        {
            StartEffect("Hello This is GGM!");
        }
    }

    public void StartEffect(string text)
    {
        _tmpText.SetText(text);
        _tmpText.ForceMeshUpdate();
        _tmpText.maxVisibleCharacters = 0;

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        TMP_TextInfo textInfo = _tmpText.textInfo;
        for(int i = 0; i < textInfo.characterCount; ++i)
        {
            yield return StartCoroutine(TypeOneCharCoroutine(textInfo, i));
        }

        _isType = false;
    }

    private IEnumerator TypeOneCharCoroutine(TMP_TextInfo textInfo, int index)
    {
        _tmpText.maxVisibleCharacters = index + 1;
        //바로 들어가면 반영이 안된다.
        _tmpText.ForceMeshUpdate();
        

        TMP_CharacterInfo charInfo = textInfo.characterInfo[index];
        

        if(charInfo.isVisible == false)
        {
            yield return new WaitForSeconds(_oneCharacterTime);
        }
        else
        {
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            Color32[] vertexColor = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            int vIndex0 = charInfo.vertexIndex;
            int vIndex1 = vIndex0 + 1;
            int vIndex2 = vIndex0 + 2;
            int vIndex3 = vIndex0 + 3;

            Vector3 v1Origin = vertices[vIndex1]; //좌측 상단
            Vector3 v2Origin = vertices[vIndex2]; //우측 상단

            float currentTime = 0;
            float percent = 0;
            while(percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / _oneCharacterTime;

                float yDelta = Mathf.Lerp(2f, 0, percent);

                vertices[vIndex1] = v1Origin + new Vector3(0, yDelta, 0);
                vertices[vIndex2] = v2Origin + new Vector3(0, yDelta, 0);

                for(int i = 0; i < 4; ++i)
                {
                    vertexColor[vIndex0 + i] = Color.Lerp(_startColor, _endColor, percent);
                }

                _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);
                yield return null;
            }
            vertices[vIndex1] = v1Origin;
            vertices[vIndex2] = v2Origin;

            for (int i = 0; i < 4; ++i)
            {
                vertexColor[vIndex0 + i] = _endColor;
            }


            _tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);
        }        
    }

    
}
