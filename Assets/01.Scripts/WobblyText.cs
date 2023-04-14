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
        _tmpText.ForceMeshUpdate(); //�޽ø� ������Ʈ(�ؽ�Ʈ�� �Ҵ��ϰ� �ѹ��� ȣ������� �ùٸ��� �޽������� ����������.)
        TMP_TextInfo textInfo = _tmpText.textInfo; //�ش� �ؽ�Ʈ �޽ÿ� �ִ� �ؽ�Ʈ ����

        for(int i = 0; i < textInfo.characterCount; ++i)  //�� �ִ� ĳ������ ���� ���� for�� ����
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i]; //i��° ������ ������ ����ִ�.
            //����ü�� ������ ����, ����, �÷����� ���� �����.

            if (charInfo.isVisible == false)
                continue; //������ �ʴ� ���� ���� ���ڵ��� ����

            //�ؽ�Ʈ �޽��� �޽��������� ���� �����ϰ� �ִ� ��Ƽ������ �����ͼ� �ű⼭ ���������� �����´�.
            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            //��Ƽ������ �������� �������� �����ϱ�. ���⼭�� ������ �Ѱ��� �ε����� 0�� ���ðŴ�.

            //�� ������ ��ȸ
            
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

        //������ �ִ� ��� �޽ÿ� ���ؼ� ������Ʈ, �޽ð� �Ѱ��� ���ݰ��� ���� for���� �ѹ��� ���ư���. 
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices; //�巡��Ʈ ������ Working���� �Ű��ش�.

            _tmpText.UpdateGeometry(meshInfo.mesh, i); //i��° �޽��� ������ ������Ʈ �Ѵ�.
        }

        //_tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);
    }
}
