using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherType 
{
    None = 0, 
    Rainy,
    Snow
}

public class MapWeather : MonoBehaviour
{
    [SerializeField] 
    private WeatherType nowWeatherType;
    [SerializeField]
    private Transform weatherParent;
    [SerializeField]
    private ParticleSystem[] weatherParticle;   // 날씨 파티클
    [SerializeField]
    private ParticleSystem nowWeatherParticle;      // 현재 날씨

    private void Start()
    {
        weatherParticle = new ParticleSystem[weatherParent.childCount];
        for(int i = 0; i < weatherParent.childCount; i++) 
        {
            weatherParticle[i] = weatherParent.GetChild(i).GetComponent<ParticleSystem>();
            weatherParticle[i].Stop();
        }
    }

    public void ChangeWeather(WeatherType type) 
    {
        if ((int)type > weatherParticle.Length)
            return;

        if (nowWeatherParticle != null)
        {
            // 현재 날씨의 loop를 멈추고
            // 일정 시간 후 끄기
           StartCoroutine(Test(nowWeatherParticle));
        }

        // 현재 날씨 
        nowWeatherParticle = weatherParticle[(int)type];

        // 현재 날씨 켜기, 플레이
        // nowWeatherParticle.gameObject.SetActive(true);
        nowWeatherParticle.Play();

        if (type != WeatherType.None)
            ChangeParticleLoop(nowWeatherParticle, true);

        // 현재날씨 타입 지정
        nowWeatherType = type;


    }

    IEnumerator Test(ParticleSystem system) 
    {
        ChangeParticleLoop(system, false);

        yield return new WaitForSeconds(5f);

        // 현재 켜져있는 파티클 종료 , 끄기 
        system.Stop();
        // system.gameObject.SetActive(false);

        yield break;
    }

    // 파티클 시스템 루프 설정 
    private void ChangeParticleLoop(ParticleSystem system , bool flag) 
    {
        var main = system.main;
        main.loop = flag;
    }

}
