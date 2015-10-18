using UnityEngine;
using System.Collections;

/// <summary>
/// Ubisenseからの座標値を取得し、Unity上の座標に変換し、シーン上のオブジェクトを移動させる
/// その座標値からさらにSDMの座標に変換し、SDMの音源を鳴らす
/// </summary>
public class UbisenseToSDM : MonoBehaviour
{
	//再生するサウンドをインスペクターから指定できるぞ！
	public SoundType _playSoundType = SoundType.Robot;
	//キューブのトランスフォームだよ
	public Transform _cubeTransform;
	private Transform _cubeTransform_; //一期前
	//サウンドのボリューム(0~127の値を指定できるよ)
	[Range (0, 127)]
	public int _soundVolume = 100;
	//Cubeが動くスピード
	public int _moveSpeed = 5;

	private int count = 0;

	void Awake(){
		_cubeTransform_ = _cubeTransform;
	}

	void Start ()
	{
		//初期音量を設定するよ
		SDMManager.Instance.ChangeVolume (_playSoundType, _soundVolume);

		SDMManager.Instance.Play (_playSoundType);

		//Ubisense
		UbisenseManager.Instance.OnUpdateUbisense05 += ((data) => {
			SetPosition(data);
		});
	}

	// Update is called once per frame
	void Update ()
	{
		count++;
		if (count > 50)  {
			//音を再生
			PlaySound ();
			count = 0;
		}

		//SDMサウンドをCubeの位置に合わせて移動させる
		SoundMoveAdjustCube ();

	}

	/// <summary>
	/// キューブが移動したら音を再生するよ
	/// </summary>
	void PlaySound ()
	{

		SDMManager.Instance.Play (_playSoundType);
		//キャラクターが移動してた場合は音を再生するよ
		//if (_cubeTransform_.position != _cubeTransform.position) {
		//	_cubeTransform_.position = _cubeTransform.position;
			//SDMManager.Instance.Play (_playSoundType);
		//}
	}

	//音源をキャラクターの位置とリンクさせるよ
	void SoundMoveAdjustCube ()
	{
		//Unity座標をSDM座標に変換
		Vector3 vec = ConvertUnityToSDM(_cubeTransform.position);
		SDMManager.Instance.SdmMove3D ((int)vec.x, (int)vec.y, (int)vec.z);
	}

	//Unity座標をSDM座標に変換
	Vector3 ConvertUnityToSDM(Vector3 pos){
		//SDMの真ん中を原点とする座標に変換
		Vector3 vec = new Vector3(pos.x-15.1f, pos.y, pos.z-4.8f);
		//範囲から外れていたら境界上まで戻す
		float r = 4.0f;
		if(vec.x > r){
			vec.x = r;
		}else if(vec.x < -r){
			vec.x = -r;
		}
		if(vec.y > r/2.0f){
			vec.y = r/2.0f;
		}else if(vec.y < 0.0f){
			vec.y = 0.0f;
		}
		if(vec.z > r){
			vec.z = r;
		}else if(vec.z < -r){
			vec.z = -r;
		}

		//Unity座標とSDM座標の向きが違うので変換
		vec.x = -vec.x;
		vec.z = -vec.z;

		//正規化（0〜127）
		vec.x = (vec.x + r) / (r*2.0f) * 127.0f;
		vec.y = (vec.y) / (r/2.0f) * 127.0f;
		vec.z = (vec.z + r) / (r*2.0f) * 127.0f;

		return vec;
	}

	//Ubisenseの座標をセット
	void SetPosition(UbisenseData data){
		data = ConvertUbisenseToUnity(data); //座標変換
		_cubeTransform.position = new Vector3(data.x, data.y, data.z);
	}

	//Ubisense座標をUnityの座標に変換
	UbisenseData ConvertUbisenseToUnity(UbisenseData data){
		float x = data.x;
		float y = data.y;
		float z = data.z;
		data.x = 17.75f - y;
		data.y = z;
		data.z = x;
		return data;
	}
}
