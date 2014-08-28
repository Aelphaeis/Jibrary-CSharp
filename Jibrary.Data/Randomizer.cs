﻿using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;


namespace Jibrary.Data
{
    public class Randomizer : IDisposable
    {
        bool disposed;
        RandomNumberGenerator rng;

        public Randomizer()
        {
            disposed = false;
            rng = new RNGCryptoServiceProvider();
        }
        public Randomizer(RandomNumberGenerator NumberGenerator)
        {
            rng = NumberGenerator;
        }

        public void FillProperties(Object obj, BindingFlags flags = BindingFlags.Default)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (typeof(Int32) == (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType))
                    property.SetValue(obj, GetInt32());
                if (typeof(string) == (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType))
                    property.SetValue(obj, GetString());
                if (typeof(DateTime) == (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType))
                    property.SetValue(obj, GetDateTime());
            }
        }

        public void FillFields(Object obj, BindingFlags flags = BindingFlags.Default)
        {
            foreach (var field in obj.GetType().GetFields())
            {
                if (typeof(Int32) == (Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType))
                    field.SetValue(obj, GetInt32()); 
                if (typeof(string) == (Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType))
                    field.SetValue(obj, GetString());
                if (typeof(DateTime) == (Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType))
                    field.SetValue(obj, GetDateTime());
            }
        }

        public void Fill(Object obj, BindingFlags flags = BindingFlags.Default)
        {
            FillProperties(obj, flags);
            FillFields(obj, flags);
        }

        public Int32 GetInt32()
        {
            Byte[] buffer = GetBytes(sizeof(int));
            return BitConverter.ToInt32(buffer, 0);
        }

        public String GetString(int size = 8)
        {
            Byte[] buffer = GetBytes(size);
            return Encoding.Default.GetString(buffer);
        }

        public DateTime GetDateTime()
        {
            return GetDateTime(DateTime.MinValue);
        }
        public DateTime GetDateTime(DateTime minValue)
        {
            return GetDateTime(minValue, DateTime.MaxValue);
        }
        public DateTime GetDateTime(DateTime minValue, DateTime maxValue)
        {
            int range = (maxValue - minValue).Days;
            range = Math.Abs(GetInt32()) % range;
            return minValue.AddDays(range);
        }   

        public Byte[] GetBytes(int size)
        {
            Byte[] buffer = new Byte[size];
            rng.GetBytes(buffer);
            return buffer;
        }

        static object GetType(String Object)
        {
            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (rng != null) 
                    rng.Dispose();
            rng = null;
            disposed = true;

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}