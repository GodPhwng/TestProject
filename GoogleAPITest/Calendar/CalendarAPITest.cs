using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;

namespace TestProject.GoogleAPITest.Calendar
{
    /// <summary>
    /// カレンダーAPIテストクラス
    /// </summary>
    public class CalendarAPITest : GoogleAPIBase<CalendarService>
    {
        /// <summary>
        /// アプリケーション名
        /// </summary>
        private const string APP_NAME = "Google Calendar API .NET";

        /// <summary>
        /// カレンダーテストクラス
        /// </summary>
        public CalendarAPITest(string keyJsonPath) : base(keyJsonPath, new string[] { CalendarService.Scope.Calendar })
        {
        }

        /// <summary>
        /// クライアントサービス作成
        /// </summary>
        /// <param name="credential">認証情報</param>
        /// <returns>クライアントサービスインターフェース</returns>
        protected override CalendarService CreateService(ICredential credential)
        {
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APP_NAME
            });
        }

        /// <summary>
        /// 予定読み取り
        /// </summary>
        /// <param name="calendarId">カレンダーID</param>
        public void ReadEvents(string calendarId)
        {
            // ここで第2引数にサービスアカウントに公開したカレンダーIDを指定する
            var request = new EventsResource.ListRequest(this.Serive, calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            var events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    var when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }

                    Console.WriteLine("{0} start：({1}) end：({2})", eventItem.Summary, when, eventItem.End.DateTime.ToString());
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
        }

        /// <summary>
        /// カレンダーイベントを追加
        /// </summary>
        /// <param name="calendarId">カレンダーID</param>
        /// <returns>イベント</returns>
        public Event InsertEvent(string calendarId)
        {
            var newEvent = new Event()
            {
                Summary = "Google I/O 2020",
                Location = "神奈川県横浜市",
                Description = "テスト備考",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2020/02/28 9:00:00"),
                    TimeZone = "Asia/Tokyo",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2020/02/28 17:00:00"),
                    TimeZone = "Asia/Tokyo",
                },

                //以下があるとエラーになるので・・・
                //Recurrence = new string[] { "RRULE:FREQ=DAILY;COUNT=2" },
                //Attendees = new EventAttendee[] {
                //    new EventAttendee() { Email = "lpage@example.com" },
                //    new EventAttendee() { Email = "sbrin@example.com" },
                //},
                //Reminders = new Event.RemindersData()
                //{
                //    UseDefault = false,
                //    Overrides = new EventReminder[] {
                //        new EventReminder() { Method = "email", Minutes = 24 * 60 },
                //        new EventReminder() { Method = "sms", Minutes = 10 },
                //    }
                //}
            };

            var request = this.Serive.Events.Insert(newEvent, calendarId);
            var createdEvent = request.Execute();
            Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);

            return createdEvent;
        }

        /// <summary>
        /// イベント更新
        /// </summary>
        /// <param name="calendarId">カレンダーID</param>
        /// <param name="evt">更新対象イベント</param>
        /// <returns>更新後のイベント</returns>
        public Event UpdateEvent(string calendarId, Event evt)
        {
            evt.Summary = "Google I/O 2020 update";
            evt.Location = "東京都八王子市";
            evt.Start.DateTime = DateTime.Parse("2020/02/28 12:00:00");
            evt.End.DateTime = DateTime.Parse("2020/02/28 21:00:00");

            var request = this.Serive.Events.Update(evt, calendarId, evt.Id);

            return request.Execute();
        }

        /// <summary>
        /// イベント削除
        /// </summary>
        /// <param name="calendarId">カレンダーID</param>
        /// <param name="eventId">イベントID</param>
        public void DeleteEvent(string calendarId, string eventId)
        {
            var request = this.Serive.Events.Delete(calendarId, eventId);
            request.Execute();
        }
    }
}
