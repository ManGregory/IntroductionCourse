using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebLMS.Models.ViewModel;

namespace WebLMS.Views
{
    public static class HtmlHelpers
    {
        public static IHtmlContent HomeworkStatusBadge(this IHtmlHelper htmlHelper, HomeworkStatus homeworkStatus)
        {
            var titlesMap = new Dictionary<HomeworkStatus, string>()
            {
                [HomeworkStatus.Passed] = "Успех",
                [HomeworkStatus.Failed] = "Есть ошибки",
                [HomeworkStatus.NoRun] = "Не пробовал",
                [HomeworkStatus.NoTests] = "Нет тестов"
            };
            var badgeClassMap = new Dictionary<HomeworkStatus, string>()
            {
                [HomeworkStatus.Passed] = "badge-success",
                [HomeworkStatus.Failed] = "badge-warning",
                [HomeworkStatus.NoRun] = "badge-info",
                [HomeworkStatus.NoTests] = "badge-dark"
            };
            string statusBadgeHtml = $"<span class='badge {badgeClassMap[homeworkStatus]}'>{titlesMap[homeworkStatus]}</span>";
            return new HtmlString(statusBadgeHtml);
        }
    }
}
