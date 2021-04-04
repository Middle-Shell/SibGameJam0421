using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Работа с интерфейсами
using UnityEngine.SceneManagement; //Работа со сценами
using UnityEngine.Audio; //Работа с аудио

public class menu : MonoBehaviour
{
    public bool isOpened = false; //Открыто ли меню
    public float volume = 0; //Громкость
    public int quality = 0; //Качество
    public bool isFullscreen = false; //Полноэкранный режим
    public AudioMixer audioMixer; //Регулятор громкости
    public Dropdown resolutionDropdown; //Список с разрешениями для игры
    private Resolution[] resolutions; //Список доступных разрешений
    private int currResolutionIndex = 0; //Текущее разрешение
    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions(); //Удаление старых пунктов
        resolutions = Screen.resolutions; //Получение доступных разрешений
        List<string> options = new List<string>(); //Создание списка со строковыми значениями

        for (int i = 0; i < resolutions.Length; i++) //Поочерёдная работа с каждым разрешением
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //Создание строки для списка
            options.Add(option); //Добавление строки в список

            if (resolutions[i].Equals(Screen.currentResolution)) //Если текущее разрешение равно проверяемому
            {
                currResolutionIndex = i; //То получается его индекс
            }
        }

        resolutionDropdown.AddOptions(options); //Добавление элементов в выпадающий список
        resolutionDropdown.value = currResolutionIndex; //Выделение пункта с текущим разрешением
        resolutionDropdown.RefreshShownValue(); //Обновление отображаемого значения
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToMain()
    {
        SceneManager.LoadScene("lvl0"); //Переход на сцену с названием Menu
    }
    public void QuitGame()
    {
        Application.Quit(); //Закрытие игры. В редакторе, кончено, она закрыта не будет, поэтому для проверки можно использовать Debug.Log();
    }
    public void ChangeVolume(float val) //Изменение звука
    {
        volume = val;
    }

    public void ChangeResolution(int index) //Изменение разрешения
    {
        currResolutionIndex = index;
    }

    public void ChangeFullscreenMode(bool val) //Включение или отключение полноэкранного режима
    {
        isFullscreen = val;
    }

    public void ChangeQuality(int index) //Изменение качества
    {
        quality = index;
    }
    public void SaveSettings()
    {
        audioMixer.SetFloat("MasterVolume", volume); //Изменение уровня громкости
        QualitySettings.SetQualityLevel(quality); //Изменение качества
        Screen.fullScreen = isFullscreen; //Включение или отключение полноэкранного режима
        Screen.SetResolution(Screen.resolutions[currResolutionIndex].width, Screen.resolutions[currResolutionIndex].height, isFullscreen); //Изменения разрешения
    }
}
