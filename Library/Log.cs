using System;


namespace Library
{
	public static class Log 
	{
		public static void DebugFrom(string message, string fileName = "") => LogRepository.GetFor(fileName).Debug(message);
		public static void DebugFrom(string message, Exception exception, string fileName = "") => LogRepository.GetFor(fileName).Debug(message, exception);
		public static void InfoFrom(string message, string fileName = "") => LogRepository.GetFor(fileName).Debug(message);
		public static void InfoFrom(string message, Exception exception, string fileName = "") => LogRepository.GetFor(fileName).Info(message, exception);
		public static void WarnFrom(string message, string fileName = "") => LogRepository.GetFor(fileName).Debug(message);
		public static void WarnFrom(string message, Exception exception, string fileName = "") => LogRepository.GetFor(fileName).Warn(message, exception);
		public static void ErrorFrom(string message, string fileName = "") => LogRepository.GetFor(fileName).Debug(message);
		public static void ErrorFrom(string message, Exception exception, string fileName = "") => LogRepository.GetFor(fileName).Error(message, exception);
		public static void FatalFrom(string message, string fileName = "") => LogRepository.GetFor(fileName).Debug(message);
		public static void FatalFrom(string message, Exception exception, string fileName = "") => LogRepository.GetFor(fileName).Fatal(message, exception);
		
		public static void Debug(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Debug(message);
		public static void Debug(string message, Exception exception, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Debug(message, exception);
		public static void Info(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Info(message);
		public static void Info(string message, Exception exception, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Info(message, exception);
		public static void Warn(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Warn(message);
		public static void Warn(string message, Exception exception, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Warn(message, exception);
		public static void Error(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Error(message);
		public static void Error(string message, Exception exception, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Error(message, exception);
		public static void Fatal(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Fatal(message);
		public static void Fatal(string message, Exception exception, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "") => LogRepository.GetFor(fileName).Fatal(message, exception);
	}
}