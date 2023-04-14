using UnityEngine;
using TMPro;

public class WobblyText : MonoBehaviour
{
    private TMP_Text _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        _tmpText.ForceMeshUpdate(); //메시를 업데이트(텍스트를 할당하고 한번은 호출해줘야 올바르게 메시정보가 가져와진다.)
        TMP_TextInfo textInfo = _tmpText.textInfo; //해당 텍스트 메시에 있는 텍스트 정보

        for(int i = 0; i < textInfo.characterCount; ++i)  //들어가 있는 캐릭터의 수에 따라 for문 수행
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i]; //i번째 글자의 정보가 들어있다.
            //구조체로 글자의 정보, 비율, 컬러등의 값이 저장됨.

            if (charInfo.isVisible == false)
                continue; //보이지 않는 띄어쓰기 같은 글자들은 생략

            //텍스트 메시의 메시정보에서 현재 참조하고 있는 매티리얼을 가져와서 거기서 정점정보를 가져온다.
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            //메티리얼이 여러개가 있을수도 있으니까. 여기서는 무조건 한개라서 인덱스는 0만 나올거다.

            //각 정점을 순회
            
            int vIndex0 = charInfo.vertexIndex;
            for (int j = 0; j < 4; ++j)
            {
                Vector3 origin = vertices[vIndex0 + j];
                vertices[vIndex0 + j] = origin + new Vector3(0, Mathf.Sin(Time.time * 2f + origin.x ), 0);
            }
            if(i == 6)
            {
                Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
                for(int j = 0; j < 4; ++j)
                {
                    vertexColors[vIndex0 + j] = Color.red;
                }
            }
        }

        //가지고 있는 모든 메시에 대해서 업데이트, 메시가 한개인 지금같은 경우는 for문은 한번만 돌아간다. 
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices; //드래프트 내용을 Working으로 옮겨준다.

            _tmpText.UpdateGeometry(meshInfo.mesh, i); //i번째 메시의 인포를 업데이트 한다.
        }

        //_tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);
    }
}
