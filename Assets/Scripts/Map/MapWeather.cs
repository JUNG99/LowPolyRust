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
    private ParticleSystem[] weatherParticle;   // ���� ��ƼŬ
    [SerializeField]
    private ParticleSystem nowWeatherParticle;      // ���� ����

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
            // ���� ������ loop�� ���߰�
            // ���� �ð� �� ����
           StartCoroutine(Test(nowWeatherParticle));
        }

        // ���� ���� 
        nowWeatherParticle = weatherParticle[(int)type];

        // ���� ���� �ѱ�, �÷���
        // nowWeatherParticle.gameObject.SetActive(true);
        nowWeatherParticle.Play();

        if (type != WeatherType.None)
            ChangeParticleLoop(nowWeatherParticle, true);

        // ���糯�� Ÿ�� ����
        nowWeatherType = type;


    }

    IEnumerator Test(ParticleSystem system) 
    {
        ChangeParticleLoop(system, false);

        yield return new WaitForSeconds(5f);

        // ���� �����ִ� ��ƼŬ ���� , ���� 
        system.Stop();
        // system.gameObject.SetActive(false);

        yield break;
    }

    // ��ƼŬ �ý��� ���� ���� 
    private void ChangeParticleLoop(ParticleSystem system , bool flag) 
    {
        var main = system.main;
        main.loop = flag;
    }

}
