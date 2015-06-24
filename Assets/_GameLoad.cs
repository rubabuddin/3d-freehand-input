using UnityEngine; 
using System.Collections; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class _GameLoad: MonoBehaviour { 
	
	// An example where the encoding can be found is at 
	// http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp 
	// We will just use the KISS method and cheat a little and use 
	// the examples from the web page since they are fully described 
	
	// This is our local private members 
	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 
	//public GameObject _Player = transform; 
	UserData myData; 
	string _PlayerName; 
	string _data; 
	string[] allValues = new string[7000];

	int LoadXMLComplete = 0;
	
	Vector3 VPosition;
	Vector3 VRotation;
	
	// When the EGO is instansiated the Start will trigger 
	// so we setup our initial values for our local members 
	void Start () { 
		// Where we want to save and load to and from 
		_FileLocation=Application.dataPath; 
		_FileName="SaveData14-41-06.txt"; 
		
		_PlayerName = "Camera Location frame by frame"; 
		
		// we need soemthing to store the information into 
		myData=new UserData(); 
		
		if (LoadXMLComplete == 0) 
		{
			LoadXML ();
			Debug.Log ("XML loaded");
			LoadXMLComplete = 1;
		}
	} 
	
	void Update () {
		if(LoadXMLComplete == 1)
		{
			Debug.Log ("Now on Frame: " + Time.frameCount);
			LoadPref();
		}
	} 
		
	void LoadPref(){
		// Load our UserData into myData 
		//LoadXML();
		myData = (UserData)DeserializeObject(allValues[Time.frameCount]); 
		Debug.Log("Loading the line with " + allValues[Time.frameCount]); 
		// set the players position to the data we loaded 
		VPosition = new Vector3(myData._iUser.x,myData._iUser.y,myData._iUser.z);  
		//VRotation = new Vector3 (myData._iUser.x_rotation, myData._iUser.y_rotation, myData._iUser.z_rotation);
		VRotation = new Vector3 (0, 0, 0);
		transform.position=VPosition; 
		transform.eulerAngles=VRotation;
		
	}

	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 
	
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 
	
	// Here we serialize our UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	// Here we deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	} 
	
	void LoadXML() 
	{ 
		int i = 0;
		StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName); 
		string _info; //= r.ReadToEnd();
		while((_info = r.ReadLine()) != null)
		{
			allValues[i] = _info;
			i++;
		}
		r.Close(); 
		_data=_info; 
		Debug.Log("File Read, first item is " + allValues[0] + " second item is " + allValues[1]); 
	} 
} 