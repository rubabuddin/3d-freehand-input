using UnityEngine; 
using System.Globalization;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class _GameSave: MonoBehaviour { 
	
	// An example where the encoding can be found is at 
	// http://www.eggheadcafe.com/articles/system.xml.xmlserialization.asp 
	// We will just use the KISS method and cheat a little and use 
	// the examples from the web page since they are fully described 
	
	// This is our local private members 
	Rect _Save, _Load, _SaveMSG, _LoadMSG; 
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 
	public GameObject _Player; 
	UserData myData; 
	string _PlayerName; 
	string _data; 
	string [] allValues;
	private bool startRecord=false;
	
	Vector3 VPosition; 
	
	// When the EGO is instansiated the Start will trigger 
	// so we setup our initial values for our local members 
	void Start () {
		// Where we want to save and load to and from 
		_FileLocation=Application.dataPath; 
		_FileName="SaveData" + System.DateTime.Now.ToString ("HH-mm-ss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".txt"; 
		
		_PlayerName = "CameraLocation"; 
		
		// we need soemthing to store the information into 
		myData=new UserData(); 
	} 
	
	void Update () {
		//choose if recording
		if (Input.GetKeyDown ("p")) {
			startRecord = true;
		} else if (Input.GetKeyDown ("s")) {
			startRecord=false;
		}
		if (startRecord) {
			SavePref ();
			Debug.Log ("Saving data");
		}
	} 
	
	void SavePref(){
		myData._iUser.x=_Player.transform.position.x; 
		myData._iUser.y=_Player.transform.position.y; 
		myData._iUser.z=_Player.transform.position.z;
		myData._iUser.x_rotation=_Player.transform.eulerAngles.x; 
		myData._iUser.y_rotation=_Player.transform.eulerAngles.y; 
		myData._iUser.z_rotation=_Player.transform.eulerAngles.z; 
		myData._iUser.name=_PlayerName;
		myData._iUser.timestamp = System.DateTime.Now.ToString ("HH:mm:ss.fff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
		
		// Time to creat our XML! 
		_data = SerializeObject(myData); 
		// This is the final resulting XML from the serialization process 
		CreateXML(); 
		Debug.Log(_data); 
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
	
	// Finally our save and load methods for the file itself 
	void CreateXML() 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			//t.Delete(); 
			writer = t.AppendText(); 
		} 
		writer.Write(_data); 
		writer.Write ("\n");
		writer.Close(); 
		Debug.Log("File written."); 
	} 
} 

// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData 
{ 
	// We have to define a default instance of the structure 
	public DemoData _iUser; 
	// Default constructor doesn't really do anything at the moment 
	public UserData() { } 
	
	// Anything we want to store in the XML file, we define it here 
	public struct DemoData 
	{ 
		public float x; 
		public float y; 
		public float z; 
		public float x_rotation; 
		public float y_rotation; 
		public float z_rotation;
		public string name;
		public string timestamp;
	} 
}