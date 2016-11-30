using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// タッチ座標をスクリーンの座標からゲーム座標に変換する
/// </summary>
public static class ExTouch
{
    //click.x == 1080 * touch.x /scw;
    //click.y == 1920 * touch.y /sch;
    static public Vector3 ConvertFHD(this Vector3 pos)
    {
        var x = 1080f * pos.x / (float)Screen.width;
        var y = 1920f * pos.y / (float)Screen.height;
        return new Vector3( x, y, pos.z);
    }
}


public class ProfileSelect : MonoBehaviour
{

    //中央のプロフィール
    public int SelectIndex { get; private set; }

    [SerializeField, Tooltip("コントロールする参照")]
    List<ProfileImage> ImageScripts;

    [SerializeField, Tooltip("スワイプ用のタッチ範囲を持ってるパネル")]
    RectTransform SwipePanel;

    [SerializeField, Tooltip("スワイプの移動感度")]
    float sensiv;

    //スワイプ判定の左下の座標
    Vector3 posA;

    //スワイプ判定の右上の座標
    Vector3 posB;

    //※未使用
    Vector3 swipeVelocity;

	//中央に近いほどプロフィールの拡大
	[SerializeField, Tooltip("拡大の最大倍率")]
	float sizeMax;

	//float initalExpantionDistance;
	[SerializeField, Tooltip("拡大が始まる距離")]
	float iniExDis;

    [SerializeField, Tooltip("プロフィールごとの幅")]
    float profilesWidth;

	[SerializeField]
	float tapMovTime;

    //コルーチンが動いてる間
    bool isHold;
	public bool IsHold
	{
		get { return isHold; }
	}


	//タッチがスライドしてるかどうか
    bool isSlide;

	//移動時間をカウント
    int movCount;

	//現在のインデックス
	int idx;

	//タッチ移動時の移動方向
	//連続でタッチされた時にチェックして同じ方向ならば、移動させずに移動回数を増やす
	Vector2 movDir;


	void Awake()
	{
		//Debug.Log("ProfileSelect.Awake");
	}

	void Start ()
    {
		
		//スワイプパネルの判定幅の確認
		//Debug.Log("anPos : " + SwipePanel.anchoredPosition.ToString());
		//Debug.Log("anMin : " + SwipePanel.anchorMin.ToString());
		//Debug.Log("anMax : " + SwipePanel.anchorMax.ToString());
		//Debug.Log("offMin : " + SwipePanel.offsetMin);
		//Debug.Log("offMax : " + SwipePanel.offsetMax);
		//Debug.Log("lolPos : " + SwipePanel.localPosition);
		//Debug.Log("Pivot : " + SwipePanel.pivot);
		//Debug.Log("size : " + SwipePanel.sizeDelta);

		//スワイプパネルの中央の座標
		var center = new Vector2(1080f / 2f, 1920f / 2f);// + SwipePanel.anchoredPosition;

		//Debug.Log( "center : " + center);

		posA = center + SwipePanel.offsetMin;
		posB = center + SwipePanel.offsetMax;

		var h = Screen.height;
		var w = Screen.width;

		//Debug.Log("ScreenSize : " + "( " + w+ ", " +h+ " )"  );

		SelectIndex = 0;

		/////////////////////////////////////////////////////////////////////////////
		//中央に一番近いやつをさがす
		var nearX = float.MaxValue;
		var nearIdx = -1;
		for (int i = 0; i < ImageScripts.Count; i++)
		{
			//var x = ImageScripts[i].RectTrans.anchoredPosition.x - center.x;
			var x = ImageScripts[i].RectTrans.anchoredPosition.x;
			var AbsX = Mathf.Abs(x);

			if (AbsX < nearX)
			{
				//Debug.Log("Abs:" + AbsX + " Near:" + nearX);
				//Debug.Log("Index : " + i);
				nearX = AbsX;
				nearIdx = i;
			}

			//サイズ変更
		}
		/////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////
		var num_ = 5;
		var startIndex_ = -2;
		var CurIndexs_ = new int[num_];
		for (int i = 0; i < num_; i++)
		{
			var idx_ = nearIdx + (i + startIndex_);
			if (idx_ < 0) idx_ = ImageScripts.Count + idx_;
			if (idx_ >= ImageScripts.Count) idx_ = idx_ - num_;

			CurIndexs_[i] = idx_;
		}

		var centerPos_ = new Vector2(0, 0);
		for (int i = 0; i < num_; i++)
		{
			ImageScripts[CurIndexs_[i]].RectTrans.anchoredPosition = centerPos_ + (new Vector2(profilesWidth, 0) * (float)(i + startIndex_));
		}
		/////////////////////////////////////////////////////////////////////////////

	}

	void Update ()
    {

        //タッチしたとき
        if(InputManager.Instance.IsTouchBegan())
        {
            StartCoroutine(CheckSwipe());
        }

        //パネルにタッチしたとき
        if (IsTouchDownSwipePanel())
        {
            //StartCoroutine(MoveProfile(sensiv));
            StartCoroutine(TouchMove2());
        }


		//スケーリング
		UpdataScalling();

    }

	/// <summary>
	/// プロファイルの座標をもどす
	/// ※コルーチンが中断されていて初期化が必要な時がある
	/// </summary>
	public void Reset()
	{
		//Debug.Log("ProfileSelect Reset");

		//中央に一番近いやつをさがす
		var nearX = float.MaxValue;
		var nearIdx = -1;
		for (int i = 0; i < ImageScripts.Count; i++)
		{
			//var x = ImageScripts[i].RectTrans.anchoredPosition.x - center.x;
			var x = ImageScripts[i].RectTrans.anchoredPosition.x;
			var AbsX = Mathf.Abs(x);

			if (AbsX < nearX)
			{
				//Debug.Log("Abs:" + AbsX + " Near:" + nearX);
				//Debug.Log("Index : " + i);
				nearX = AbsX;
				nearIdx = i;
			}
			
		}

		var num = 5;
		var startIndex = -2;
		var CurIndexs = new int[num];
		for (int i = 0; i < num; i++)
		{
			var idx = nearIdx + (i + startIndex);
			if (idx < 0) idx = ImageScripts.Count + idx;
			if (idx >= ImageScripts.Count) idx = idx - num;

			CurIndexs[i] = idx;
		}

		var centerPos = new Vector2(0, 0);
		for (int i = 0; i < num; i++)
		{
			ImageScripts[CurIndexs[i]].RectTrans.anchoredPosition = centerPos + (new Vector2(profilesWidth, 0) * (float)(i + startIndex));
		}

		//ホールド
		isHold = false;

		//移動回数
		movCount = 0;

	}

	/// <summary>
	/// 設定したタッチ判定内をタッチしているかどうか
	/// </summary>
	/// <returns></returns>
    bool IsTouchDownSwipePanel()
    {

        if (InputManager.Instance.IsTouchBegan() == false)
        {
            return false;
        }

        var touch = InputManager.Instance.GetTouchPosition().ConvertFHD();

        //Debug.Log("touch :" + touch);

        if (touch.x <= 0.0f || touch.x > 1080.0f) return false;

        //未完成
        //if (touch.y <= 0.0f) return false;
        if (touch.y <= posA.y || touch.y >= posB.y ) return false;

        return true;
    }

    
	/// <summary>
	/// スワイプしてるかどうかの判定コルーチン
	/// スワイプしている間は isSwipe が true になる
	/// </summary>
	/// <returns></returns>
    IEnumerator CheckSwipe()
    {
        var pos = InputManager.Instance.GetTouchPosition();

        //スライド判定の感度
        float sens = 0.5f;

        while (InputManager.Instance.IsTouchMoved())
        {
            yield return null;

            var v = pos - InputManager.Instance.GetTouchPosition();

            //大きさにする
            var mag = v.magnitude;

            //一定以上の大きさとの時 スライドである
            if (sens < mag)
            {
                isSlide = true;
                //Debug.Log("Slide ON");
                break;
            }
        }

        while(InputManager.Instance.IsTouchMoved())
        {
            yield return null;
        }

        //yield return new WaitForEndOfFrame();

        if (isSlide)
        {
            isSlide = false;
            //Debug.Log("Slide OFF");
        }
        
        
    }

	//未使用？
	/// <summary>
	/// タッチでプロファイルを動かすコルーチン
	/// </summary>
	/// <param name="speed"></param>
	/// <param name="turn"></param>
	/// <returns></returns>
	IEnumerator MoveProfile(float speed, bool turn = false)
    {
		Debug.Log("MoveProfile : Start");
        if (isHold) yield break;

        
        isHold = true;

        var pos = InputManager.Instance.GetTouchPosition().ConvertFHD();

        //while (true)
        while (InputManager.Instance.IsTouchEnded() == false)
        {   
            yield return null;

            if (isSlide == false) continue;

            var curPos = InputManager.Instance.GetTouchPosition().ConvertFHD();

            //移動方向に動かす
            var mov = curPos - pos;

            //mov *= sensiv;

            foreach (ProfileImage i in ImageScripts)
            {
                i.RectTrans.anchoredPosition += new Vector2(mov.x, 0);

                pos = curPos;
            }

        }

        isHold = false;

		//yield return null;
		Debug.Log("MoveProfile : End");
	}

	//未使用
	/// <summary>
	/// タッチしてる
	/// </summary>
	/// <returns></returns>
    IEnumerator TouchMove()
    {
        Debug.Log("TouchMove : Start");

        //if (isHold) yield break;
        //isHold = true;

        //スライド判定の感度
        float sens = 1.0f;

        var pos = InputManager.Instance.GetTouchPosition().ConvertFHD();

        //
        while (isSlide == false)
        {
            if ( InputManager.Instance.IsTouchMoved() == false)
            {
                yield break;
            }
            yield return null;
        }

        if (isHold) yield break;

        //タッチしている間/
        while (InputManager.Instance.IsTouchMoved())
        {
            yield return null;

            //インデックスの確認

            var curPos = InputManager.Instance.GetTouchPosition().ConvertFHD();

            //移動方向に動かす
            var mov = curPos - pos;

            //mov *= sensiv;

            foreach (ProfileImage img in ImageScripts)
            {
                img.RectTrans.anchoredPosition += new Vector2(mov.x, 0);

                pos = curPos;
            }

        }
        //中央に一番近いやつをさがす

        var nearX = float.MaxValue;
        var nearIdx = -1;
        var center = new Vector2(1080f/2, 1920f/2f);
        for (int i = 0; i <  ImageScripts.Count; i++)
        {
            //var x = ImageScripts[i].RectTrans.anchoredPosition.x - center.x;
            var x = ImageScripts[i].RectTrans.anchoredPosition.x;
            var AbsX = Mathf.Abs(x);

            if( AbsX < nearX )
            {
                //Debug.Log("Abs:"+ AbsX + " Near:"+nearX);
                //Debug.Log("Index : " + i );
                nearX = AbsX;
                nearIdx = i;
            }
        }

        //最終座標に移動
        //ImageScripts[nearIdx].RectTrans.anchoredPosition = new Vector2(0,0) ;
        for (int i = 0; i < ImageScripts.Count; i++)
        {
            var x = profilesWidth * (float)(i - nearIdx);
            ImageScripts[i].RectTrans.anchoredPosition = new Vector2(x, 0);
        }

		//isHold = false;
		//yield return null;

		Debug.Log("TouchMove : End");
	}

	/// <summary>
	/// タッチしてる
	/// </summary>
	/// <returns></returns>
	IEnumerator TouchMove2()
	{
		//Debug.Log("TouchMove2 : Start");

		//スライド判定の感度
		float sens = 0.5f;

		var pos = InputManager.Instance.GetTouchPosition().ConvertFHD();

		//スライドになるまでループする、タッチを離したら終了
		while (isSlide == false)
		{
			if (InputManager.Instance.IsTouchMoved() == false)
			{
				yield break;
			}
			yield return null;
		}

		if (isHold) yield break;

		isHold = true;

		var nearX = float.MaxValue;
		var nearIdx = -1;
		var nearIdxPrev = -1;
		var center = new Vector2(1080f / 2, 1920f / 2f);

		//タッチしている間/
		while (InputManager.Instance.IsTouchMoved())
		{
			yield return null;

			var curPos = InputManager.Instance.GetTouchPosition().ConvertFHD();

			//移動方向に動かす
			var mov = curPos - pos;

			//移動量分動かす
			foreach (ProfileImage img in ImageScripts)
			{
				img.RectTrans.anchoredPosition += new Vector2(mov.x, 0);

				pos = curPos;
			}

			nearIdxPrev = nearIdx;
			nearX = float.MaxValue;

			//中央に一番近いやつをさがす
			for (int i = 0; i < ImageScripts.Count; i++)
			{
				//var x = ImageScripts[i].RectTrans.anchoredPosition.x - center.x;
				var x = ImageScripts[i].RectTrans.anchoredPosition.x;
				var AbsX = Mathf.Abs(x);

				if (AbsX < nearX)
				{
					//Debug.Log("Abs:" + AbsX + " Near:" + nearX);
					//Debug.Log("Index : " + i);
					nearX = AbsX;
					nearIdx = i;
				}
			}

			//中央に合わせて見切れないように瞬間移動させる
			//中央に一番近いnearIdx(0~4)
			//左のやつ　nearIdx - 1
			//右のやつ　nearIdx + 1

			var num = 5;
			var startIndex = -2;
			var CurIndexs = new int[num];
			for (int i = 0; i < num; i++)
			{
				var idx = nearIdx + (i + startIndex);
				if (idx < 0) idx = ImageScripts.Count + idx;
				if (idx >= ImageScripts.Count) idx = idx - num;

				CurIndexs[i] = idx;
			}

			var centerPos = ImageScripts[nearIdx].RectTrans.anchoredPosition;
			for (int i = 0; i < num; i++)
			{
				ImageScripts[CurIndexs[i]].RectTrans.anchoredPosition = centerPos + (new Vector2(profilesWidth, 0) * (float)(i + startIndex));
			}

			//var llIdx = nearIdx - 2;
			//if (llIdx < 0) llIdx = ImageScripts.Count + llIdx;

			//var leftIdx = nearIdx - 1;
			//if (leftIdx < 0) leftIdx = ImageScripts.Count + leftIdx;

			//var cen = nearIdx;

			//var rightIdx = nearIdx + 1;
			//if (rightIdx >= ImageScripts.Count) rightIdx = rightIdx - ImageScripts.Count;

			//var rrIdx = nearIdx + 2;
			////if (rrIdx >= ImageScripts.Count) rrIdx = 1;
			//if (rrIdx >= ImageScripts.Count) rrIdx = rrIdx - ImageScripts.Count;

			
			//var centerPos = ImageScripts[nearIdx].RectTrans.anchoredPosition;

			//ImageScripts[llIdx].RectTrans.anchoredPosition = centerPos + new Vector2(-900f, 0);
			//ImageScripts[leftIdx].RectTrans.anchoredPosition = centerPos + new Vector2(-450f, 0);
			////ImageScripts[nearIdx].RectTrans.anchoredPosition = centerPos ;
			//ImageScripts[rightIdx].RectTrans.anchoredPosition = centerPos + new Vector2(450f, 0);
			//ImageScripts[rrIdx].RectTrans.anchoredPosition = centerPos + new Vector2(900f, 0);
		}

		//最終座標に移動
		var num_ = 5;
		var startIndex_ = -2;
		var CurIndexs_ = new int[num_];
		for (int i = 0; i < num_; i++)
		{
			var idx = nearIdx + (i + startIndex_);
			if (idx < 0) idx = ImageScripts.Count + idx;
			if (idx >= ImageScripts.Count) idx = idx - num_;

			CurIndexs_[i] = idx;
		}

		var centerPos_ = new Vector2(0,0);
		for (int i = 0; i < num_; i++)
		{
			ImageScripts[CurIndexs_[i]].RectTrans.anchoredPosition = centerPos_ + (new Vector2(profilesWidth, 0) * (float)(i + startIndex_));
		}

		isHold = false;
		//yield return null;
		//Debug.Log("TouchMove2 : End");
	}

	// time秒間かけてdir方向にmov距離移動する
	// 失敗
	IEnumerator MoveProfiles(Vector2 dir, float mov, float time)
    {
        //総時間
        float totalTime = 0.0f;

        //総移動量
        float totalMov = 0.0f;

        int cnt = 0;

        //
        while (totalMov <= mov)
        {

            //フレーム単位の時間の割合
            var parcentage =  Time.deltaTime / time;

            //フレーム単位の移動量
            var curMovVel = mov * parcentage;
            var curMov = dir * curMovVel;

            foreach (ProfileImage i in ImageScripts)
            {
                i.RectTrans.anchoredPosition += curMov;
            }

            totalTime += Time.deltaTime;
            totalMov += curMovVel;

            
            Debug.Log("カウント : " + cnt );
            ++cnt;

            yield return null;
        }

        var correct = dir * (mov - totalMov);


        foreach (ProfileImage i in ImageScripts)
        {
            i.RectTrans.anchoredPosition += correct;
        }

        Debug.Log("totalTime : " + totalTime);

        yield return null;
    }


	/// <summary>
	/// time秒間かけてdir方向にmov距離移動する
	/// </summary>
	/// <param name="dir">方向</param>
	/// <param name="mov">総移動量</param>
	/// <param name="time">かかる時間</param>
	/// <returns></returns>
	IEnumerator MoveProfiles2(Vector2 dir, float mov, float time)
    {

        if (isSlide)
        {
            //Debug.Log("isSlide == true : yield Break");
            yield break;
        }

        if (isHold)
        {
			if (dir.x == movDir.x)
			{
				++movCount;
			}
			//Debug.Log("ムーブカウント : " + movCount);
            yield break;
        }
        isHold = true;

		
		movDir = dir;

        //総時間
        float totalTime = 0.0f;

        //総移動量
        float totalMov = 0.0f;

        int cnt = 0;

        List<Vector2> postions = new List<Vector2>();

        var v = mov * dir;
        foreach (ProfileImage i in ImageScripts)
        {
            postions.Add(i.RectTrans.anchoredPosition + v);
        }

		//移動処理
        while (totalMov <= mov)
        {
            yield return null;

            //フレーム単位の時間の割合
            var parcentage = Time.deltaTime / time;

            //フレーム単位の移動量
            var curMovVel = mov * parcentage;
            var curMov = dir * curMovVel;

            foreach (ProfileImage i in ImageScripts)
            {
                i.RectTrans.anchoredPosition += curMov;
            }

            totalTime += Time.deltaTime;
            totalMov += curMovVel;


            //Debug.Log("カウント : " + cnt);
            ++cnt;
           
        }

		//foreach (ProfileImage i in ImageScripts)
		//{
		//    i.RectTrans.anchoredPosition = postions[????];
		//}
		var nearIdx = 0;
		var nearX = float.MaxValue;

		/////////////////////////////////////////////////////////////////////////////
		//中央に一番近いやつをさがす
		for (int i = 0; i < ImageScripts.Count; i++)
		{
			//var x = ImageScripts[i].RectTrans.anchoredPosition.x - center.x;
			var x = ImageScripts[i].RectTrans.anchoredPosition.x;
			var AbsX = Mathf.Abs(x);

			if (AbsX < nearX)
			{
				//Debug.Log("Abs:" + AbsX + " Near:" + nearX);
				//Debug.Log("Index : " + i);
				nearX = AbsX;
				nearIdx = i;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////
		var num_ = 5;
		var startIndex_ = -2;
		var CurIndexs_ = new int[num_];
		for (int i = 0; i < num_; i++)
		{
			var idx = nearIdx + (i + startIndex_);
			if (idx < 0) idx = ImageScripts.Count + idx;
			if (idx >= ImageScripts.Count) idx = idx - num_;

			CurIndexs_[i] = idx;
		}

		var centerPos_ = new Vector2(0, 0);
		for (int i = 0; i < num_; i++)
		{
			ImageScripts[CurIndexs_[i]].RectTrans.anchoredPosition = centerPos_ + (new Vector2(profilesWidth, 0) * (float)(i + startIndex_));
		}
		/////////////////////////////////////////////////////////////////////////////

		//Debug.Log("totalTime : " + totalTime);

        isHold = false;

		//連続でタッチされてるとき繰り返す
        if(movCount > 0)
        {
            --movCount;
            yield return MoveProfiles2(dir, mov, time);
        }
    }

    /// <summary>
    /// UI右の画像がクリックされたとき
    /// </summary>
    public void OnClickLeft()
    {
        //Debug.Log("Button ←");
        StartCoroutine(MoveProfiles2(Vector3.right, profilesWidth, tapMovTime));
    }

    /// <summary>
    /// UI左の画像がクリックされたとき
    /// </summary>
    public void OnClickRight()
    {
        //Debug.Log("Button →");
        StartCoroutine(MoveProfiles2(Vector3.left, profilesWidth, tapMovTime));
    }

	/// <summary>
	/// 毎フレームのProfileのスケーリングの計算
	/// </summary>
	void UpdataScalling()
	{
		//中央 x == 0
		//スケール変更距離 initalExpantionDistance
		//変更スケールの最大 sizeMax
		for (int i = 0; i < ImageScripts.Count; i++)
		{
			var dist = ImageScripts[i].RectTrans.anchoredPosition.x;
			var mag = Mathf.Abs(dist);
			if (mag < iniExDis)
			{
				var scl = 1.0f + (sizeMax - 1.0f) * (1.0f - mag / iniExDis);
				ImageScripts[i].transform.localScale = new Vector2(scl, scl);
			}
			else
			{
				ImageScripts[i].transform.localScale = Vector3.one;
			}	
		}
	}

	//※失敗
	void UpdataScalling_()
	{
		//中央 x == 0
		//スケール変更距離 initalExpantionDistance
		//変更スケールの最大 sizeMax
		for (int i = 0; i < ImageScripts.Count; i++)
		{
			var dist = ImageScripts[i].RectTrans.anchoredPosition.x;
			var mag = Mathf.Abs(dist);
			if (mag < iniExDis)
			{
				var scl = sizeMax * (1.0f - mag / iniExDis);
				ImageScripts[i].transform.localScale = new Vector2(scl, scl);
			}
			else
			{
				ImageScripts[i].transform.localScale = Vector3.one;
			}
		}
	}
}