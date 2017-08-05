#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_CREATE_LOBBY : GSTypedRequest<LogEventRequest_CREATE_LOBBY, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_CREATE_LOBBY() : base("LogEventRequest"){
			request.AddString("eventKey", "CREATE_LOBBY");
		}
		public LogEventRequest_CREATE_LOBBY Set_create_data( GSData value )
		{
			request.AddObject("create_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_CREATE_LOBBY : GSTypedRequest<LogChallengeEventRequest_CREATE_LOBBY, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_CREATE_LOBBY() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "CREATE_LOBBY");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_CREATE_LOBBY SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_CREATE_LOBBY Set_create_data( GSData value )
		{
			request.AddObject("create_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_ICON_PRESS : GSTypedRequest<LogEventRequest_ICON_PRESS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_ICON_PRESS() : base("LogEventRequest"){
			request.AddString("eventKey", "ICON_PRESS");
		}
		public LogEventRequest_ICON_PRESS Set_icon_data( GSData value )
		{
			request.AddObject("icon_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_ICON_PRESS : GSTypedRequest<LogChallengeEventRequest_ICON_PRESS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_ICON_PRESS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "ICON_PRESS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_ICON_PRESS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_ICON_PRESS Set_icon_data( GSData value )
		{
			request.AddObject("icon_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_JOIN_LOBBY : GSTypedRequest<LogEventRequest_JOIN_LOBBY, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_JOIN_LOBBY() : base("LogEventRequest"){
			request.AddString("eventKey", "JOIN_LOBBY");
		}
		public LogEventRequest_JOIN_LOBBY Set_join_data( GSData value )
		{
			request.AddObject("join_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_JOIN_LOBBY : GSTypedRequest<LogChallengeEventRequest_JOIN_LOBBY, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_JOIN_LOBBY() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "JOIN_LOBBY");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_JOIN_LOBBY SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_JOIN_LOBBY Set_join_data( GSData value )
		{
			request.AddObject("join_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_LEAVE_GAME : GSTypedRequest<LogEventRequest_LEAVE_GAME, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_LEAVE_GAME() : base("LogEventRequest"){
			request.AddString("eventKey", "LEAVE_GAME");
		}
		public LogEventRequest_LEAVE_GAME Set_leave_data( GSData value )
		{
			request.AddObject("leave_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_LEAVE_GAME : GSTypedRequest<LogChallengeEventRequest_LEAVE_GAME, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_LEAVE_GAME() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "LEAVE_GAME");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_LEAVE_GAME SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_LEAVE_GAME Set_leave_data( GSData value )
		{
			request.AddObject("leave_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_LEVEL_FINISHED : GSTypedRequest<LogEventRequest_LEVEL_FINISHED, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_LEVEL_FINISHED() : base("LogEventRequest"){
			request.AddString("eventKey", "LEVEL_FINISHED");
		}
		public LogEventRequest_LEVEL_FINISHED Set_finish_data( GSData value )
		{
			request.AddObject("finish_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_LEVEL_FINISHED : GSTypedRequest<LogChallengeEventRequest_LEVEL_FINISHED, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_LEVEL_FINISHED() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "LEVEL_FINISHED");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_LEVEL_FINISHED SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_LEVEL_FINISHED Set_finish_data( GSData value )
		{
			request.AddObject("finish_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_REFRESH_GAME : GSTypedRequest<LogEventRequest_REFRESH_GAME, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_REFRESH_GAME() : base("LogEventRequest"){
			request.AddString("eventKey", "REFRESH_GAME");
		}
		public LogEventRequest_REFRESH_GAME Set_refresh_data( GSData value )
		{
			request.AddObject("refresh_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_REFRESH_GAME : GSTypedRequest<LogChallengeEventRequest_REFRESH_GAME, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_REFRESH_GAME() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "REFRESH_GAME");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_REFRESH_GAME SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_REFRESH_GAME Set_refresh_data( GSData value )
		{
			request.AddObject("refresh_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_REFRESH_LOBBY : GSTypedRequest<LogEventRequest_REFRESH_LOBBY, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_REFRESH_LOBBY() : base("LogEventRequest"){
			request.AddString("eventKey", "REFRESH_LOBBY");
		}
		public LogEventRequest_REFRESH_LOBBY Set_refresh_data( GSData value )
		{
			request.AddObject("refresh_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_REFRESH_LOBBY : GSTypedRequest<LogChallengeEventRequest_REFRESH_LOBBY, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_REFRESH_LOBBY() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "REFRESH_LOBBY");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_REFRESH_LOBBY SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_REFRESH_LOBBY Set_refresh_data( GSData value )
		{
			request.AddObject("refresh_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_SET_ICON : GSTypedRequest<LogEventRequest_SET_ICON, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_SET_ICON() : base("LogEventRequest"){
			request.AddString("eventKey", "SET_ICON");
		}
		public LogEventRequest_SET_ICON Set_icon_data( GSData value )
		{
			request.AddObject("icon_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_SET_ICON : GSTypedRequest<LogChallengeEventRequest_SET_ICON, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_SET_ICON() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "SET_ICON");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_SET_ICON SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_SET_ICON Set_icon_data( GSData value )
		{
			request.AddObject("icon_data", value);
			return this;
		}
		
	}
	
	public class LogEventRequest_START_GAME : GSTypedRequest<LogEventRequest_START_GAME, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_START_GAME() : base("LogEventRequest"){
			request.AddString("eventKey", "START_GAME");
		}
		public LogEventRequest_START_GAME Set_start_data( GSData value )
		{
			request.AddObject("start_data", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_START_GAME : GSTypedRequest<LogChallengeEventRequest_START_GAME, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_START_GAME() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "START_GAME");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_START_GAME SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_START_GAME Set_start_data( GSData value )
		{
			request.AddObject("start_data", value);
			return this;
		}
		
	}
	
}
	

namespace GameSparks.Api.Messages {


}
