/**
 * Created by VolkovA on 28.02.14.
 */

namespace selenium.core.Framework.Service {
    public interface ServiceFactory {
        // Создать маршрутизатор страниц(сопоставление Url-->Страницы) для сервиса
        Router createRouter();
        // Создать сервис
        IService createService();
        // Паттерн для Url, которым соответствует сервис
        BaseUrlPattern createBaseUrlPattern();
        // Дефолтные параметры базового Url
        BaseUrlInfo getDefaultBaseUrlInfo();
    }
}