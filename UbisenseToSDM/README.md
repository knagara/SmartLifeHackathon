# UbisenseToSDM.cs

Ubisenseからの座標値を取得し、シーン上のオブジェクトを連動させた上で、SDMの音源を鳴らします

## How to use

MqttManager, SDMManager, UbisenseManagerを配置した上で、
2つのオブジェクトを生成し、片方はUbisenseToSDM.csをアタッチ、
もう片方は座標値に連動させて動かすオブジェクトとします。

I-REF棟の3Dモデルを配置すると、下の画像のようになります

![HowToUse](https://raw.githubusercontent.com/knagara/SmartLifeHackathon/master/UbisenseToSDM/screenshot.png)
![I-REF](https://raw.githubusercontent.com/knagara/SmartLifeHackathon/master/UbisenseToSDM/screenshot2.png)

