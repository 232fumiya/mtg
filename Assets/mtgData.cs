using UnityEngine;
using System.Collections;
using MiniJSON;
using UnityEngine.UI;
public class mtgData : MonoBehaviour {
	int setnum=0;
	string[] set = new string[]{"DTK","ORI","EMN"};
	string[] color = new string[]{"black","white","red","blue","green"};
	int colornum;
	public Dropdown setDrop;
	public Dropdown colorDrop;
	public RawImage img;
	public Text cardName;
	public Text cardText;
	string imgurl;
	string[] cardlist;
	IList[] edition;
	string[] TextList;
	int count=0;


	void Awake(){
		img.color = Color.black;
	}


	public void Search(){
		count = 0;
		StartCoroutine (access());
	}


	public void OnDropDown(){
		setnum = setDrop.value;
	}


	public void OnDropDown2(){
		colornum = colorDrop.value;
	}


	IEnumerator access(){
		string url ="https://api.deckbrew.com/mtg/cards?set="+set[setnum]+"&color="+color[colornum];
		WWW www = new WWW (url);
		yield return www;
		parseList (www.text);
		cardName.text = cardlist [0];
		cardText.text = TextList[0];
		imgurl = parse(edition[0],"image_url");
		StartCoroutine (getImg());
	}


	void parseList(string data){
		var json = (IList)MiniJSON.Json.Deserialize (data);
		cardlist =new string[json.Count];
		TextList=new string[json.Count];
		edition =new IList[json.Count];
		int i = 0;
		foreach (IDictionary jsonData in json) {
			cardlist[i] = jsonData["name"].ToString();
			TextList[i] =jsonData["text"].ToString();
			edition[i] =(IList)jsonData["editions"];
			i++;
		}
	}


	string parse (IList data,string column){
		string jsonData = "";
		foreach(IDictionary json in data)
		{
			jsonData = json[column].ToString();
		}
		return jsonData;
	}


	IEnumerator getImg(){
		string url=imgurl;
		WWW www = new WWW (url);
		yield return www;
		img.texture = www.textureNonReadable;
		img.SetNativeSize();
		img.color = Color.white;
	}


	public void Next(int value){
		count += value;
		if (count < 0)
			count = 0;
		else if (cardlist.Length <= count)
			count = cardlist.Length - 1;
		cardName.text = cardlist [count];
		cardText.text = TextList[count];
		imgurl = parse(edition[count],"image_url");
		StartCoroutine (getImg());
	}
}
