using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AttendanceSystem.Functions
{
    public class Sessions
    {
        private HttpContext _context ;
        public Sessions(HttpContext content)
        {
            this._context = content;
        }
        public void SetStringSession(string SessionName , string SessionValue)
        {
            _context.Session.SetString(SessionName, SessionValue);
        }
        public void SetIntSession(string SessionName , int Integ)
        {
            _context.Session.SetInt32(SessionName, Integ);
        }
        public string GetStringSession(string SessionName)
        {
            return _context.Session.GetString(SessionName);
        }
        public int GetIntSession(string SessionName)
        {
            return _context.Session.GetInt32(SessionName).Value;
        }

        public void SetObjectSession(string SessionName , object value)
        {
            _context.Session.SetString(SessionName, JsonConvert.SerializeObject(value));
        }
        public T GetObjectSession<T>(string SessionName)
        {
            try
            {
                var value = _context.Session.GetString(SessionName);
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }catch(Exception exp)
            {
                new Logging().WriteLog(exp.Message);
                return default(T);
            }
        }
        public void RemoveSession(string SessionName)
        {
            _context.Session.Remove(SessionName);
        }
    }
}
