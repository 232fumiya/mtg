using UnityEngine;
using System.Collections;
using System.IO;
public class MtgWisdom : MonoBehaviour {
	string[] setName=new string[]{"DTK","ORI","BFZ","OGW","SOI","EMN"};
	public int num=0;
	bool isFin=true;
	string stan;
	// Use this for initialization
	void Start(){
		StartCoroutine (Create());
	}
	IEnumerator Create () {
		while (true) {
			if (num >= setName.Length)
			{
				parse(stan);
				yield break;
			}
			if (isFin) {
				StartCoroutine (access ());
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
	IEnumerator access(){
		isFin = false;
		string url = "http://whisper.wisdom-guild.net/cardlist/"+setName[num]+".txt";
		WWW www = new WWW (url);
		yield return www;
		stan = stan + www.text;
		num++;
		isFin = true;
		//parse (www.text);
		//write (www.text);
	}
	void parse(string data){
		string path = Application.dataPath + "/CSV/Standard.csv";
		StreamWriter writer = new StreamWriter (path, true, System.Text.Encoding.GetEncoding ("utf-8"));
		string header = "セット,英語名,日本語名,パワー,タフネス,レアリティ,タイプ,マナコスト,能力";
		writer.WriteLine (header);

		string[] dataarray = data.Split ('\n');
		bool textMode = false;
		bool next = false;
		string en = "";
		string jp = "";
		string rare = "";
		string text = "";
		string P = "";
		string T = "";
		string set = "";
		string type = "";
		string cost = "";
		for (int i=1; i<dataarray.Length; i++) {
			if (dataarray [i].Contains ("英語名")) {
				en = dataarray [i].Replace ("英語名：", "");
				en = Remove (en);
			} else if (dataarray [i].Contains ("日本語名")) {
				jp = dataarray [i].Replace ("日本語名：", "");
				jp = Remove (jp);
			}  else if (dataarray [i].Contains ("セット：")) {
				set = dataarray [i].Replace ("セット：", "");
				set = Remove (set);
			} else if (dataarray [i].Contains ("コスト：")) {
				cost = dataarray [i].Replace ("コスト：", "");
				cost = cost.Replace ("(", "");
				cost = cost.Replace (")", "");
			} else if (dataarray [i].Contains ("Ｐ／Ｔ：")) {
				string[] PT = dataarray [i].Split ('/');
				PT [0] = PT [0].Replace ("Ｐ／Ｔ：", "");
				P = PT [0];
				T = PT [1];
				textMode = false;
			} else if (dataarray [i].Contains ("稀少度：")) {
				rare = dataarray [i].Replace ("稀少度：", "");
				rare = Remove (rare);
				next=true;
			} else if (dataarray [i].Contains ("タイプ：")) {
				string[] typeArray = dataarray [i].Split ('-');
				for (int j=0; j<typeArray.Length; j++) {
					if (typeArray [j].Contains ("タイプ：")) {
						type = typeArray [j].Replace ("タイプ：", "");
						type = Remove (type);
					}
				}
				textMode = true;
			} else if (dataarray [i].Contains ("イラスト：")) { 
				textMode = false;
			} else if (textMode) {
				text = text + dataarray [i];
			} else if(next)
			{
				text = Remove (text);
				string line = set+","+en + "," + jp + "," + P + "," + T + "," + rare + "," + type + "," + cost + "," + text;
				writer.WriteLine (line);
				text="";
				next=false;
			}
		}
		writer.Flush ();
		writer.Close ();
		Debug.Log ("fin");
		num++;
		isFin = true;
	}
	string Remove(string line){
		line = line.Replace (" ", "");
		line = line.Replace ("\r","");
		line = line.Replace ("\n","");
		line = line.Replace (","," ");
		return line;
	}
}
