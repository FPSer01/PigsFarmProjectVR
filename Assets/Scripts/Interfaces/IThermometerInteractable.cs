
/// <summary>
/// Интерфейс для определения объектов, с которыми может взаимодействовать термометр (класс Thermometer)
/// </summary>
public interface IThermometerInteractable
{
    /// <summary>
    /// Получить температуру объекта
    /// </summary>
    /// <returns>Температура объекта</returns>
    float GetTemperature();

    /// <summary>
    /// Подсветить объект
    /// </summary>
    /// <param name="highlight"></param>
    void HighlightInteractable(bool highlight);
}
