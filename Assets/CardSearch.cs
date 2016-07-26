using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
public class CardSearch : MonoBehaviour {
	ArrayList[] cardList;
	public TextAsset cardData;
	string[] header;
	public InputField input;
	public InputField input2;
	public Text NameTex;
	public Text AbilTex;
	ArrayList result;
	int num =0;
	// Use this for initialization
	void Start () {
		StringReader reader = new StringReader (cardData.text);
		header = reader.ReadLine().Split(',');
		cardList = new ArrayList [header.Length];
		for (int i=0; i<header.Length; i++) {
			cardList[i] =new ArrayList();
		}
		while (reader.Peek()>0) {
			string[] body = reader.ReadLine().Split(',');
			for(int i=0;i<body.Length;i++)
			{
				cardList[i].Add(body[i]);
			}
		}
	}
 	public void Search(){
		string target = input.text;
		string ability = input2.text;
		result = new ArrayList();
		if (target == ""&&ability=="")
			return;
		for (int j=0;j<cardList[0].Count;j++) {
			string name =cardList[2][j].ToString();
			string abil =cardList[8][j].ToString();
			if(name.Contains(target)&&abil.Contains(ability))
				result.Add(j);
		}
		num=0;
		View(0);
	}
	public void View(int value){
		if (result.Count <= 0)
			return;
		num += value;
		if (num <= 0)
			num = 0;
		if(result.Count<= num)
			num =result.Count-1;
		int index = (int)result [num];
		NameTex.text = cardList [2] [index].ToString ();
		AbilTex.text = cardList [8] [index].ToString ();
	}
}
