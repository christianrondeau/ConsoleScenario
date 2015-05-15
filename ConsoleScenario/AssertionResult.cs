namespace ConsoleScenario
{
	public class AssertionResult
	{
		public static AssertionResult Pass()
		{
			return new AssertionResult { Success = true };
		}

		public static AssertionResult Fail(string message, string expected = null)
		{
			return new AssertionResult { Success = false, Message = message, Expected = expected };
		}

		public bool Success { get; set; }
		public string Message { get; set; }
		public string Expected { get; set; }

		public override string ToString()
		{
			return Success ? "Success" : GetFailureString();
		}

		private string GetFailureString()
		{
			return string.Format("Failed: {0}{1}", Message, (Expected != null ? " (expected: '" + Expected + "')" : ""));
		}

		protected bool Equals(AssertionResult other)
		{
			return Success.Equals(other.Success) && string.Equals(Message, other.Message) && string.Equals(Expected, other.Expected);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((AssertionResult) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Success.GetHashCode();
				hashCode = (hashCode*397) ^ (Message != null ? Message.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ (Expected != null ? Expected.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}