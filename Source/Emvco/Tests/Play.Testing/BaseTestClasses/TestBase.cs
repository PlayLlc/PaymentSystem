using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Play.Ber.DataObjects;

using Xunit.Sdk;

namespace Play.Testing.BaseTestClasses;

public abstract class TestBase
{
    #region Instance Members

    public virtual void CustomizeModuleObjects(IFixture fixture)
    { }

    /// <summary>
    ///     This method is used to intercept exceptions that happen in the 'Arrange' section of a unit test. Expressive
    ///     messages are supported
    /// </summary>
    [SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>")]
    public static void Arrange(Action action, string message = "")
    {
        try
        {
            action.Invoke();
        }
        catch (XunitException exception)
        {
            if (message != string.Empty)
            {
                Debug.WriteLine(message);

                throw new PlayUnitTestArrangeException(message, exception);
            }

            throw new PlayUnitTestArrangeException(message, exception);
        }
        catch (Exception exception)
        {
            if (message != string.Empty)
            {
                Debug.WriteLine(message);

                throw new PlayUnitTestArrangeException(message, exception);
            }

            throw new PlayUnitTestArrangeException(message, exception);
        }
    }

    /// <summary>
    ///     This method is used to intercept exceptions that happen in the 'Act' section of a unit test. This method can be
    ///     wrapped around the system under test to provide explicit messaging for the logic being tested. Expressive messages
    ///     are supported
    /// </summary>
    [SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>")]
    public static void Act(Action action, string message = "")
    {
        try
        {
            action.Invoke();
        }
        catch (XunitException exception)
        {
            if (message != string.Empty)
            {
                Debug.WriteLine(message);

                throw new PlayUnitTestActException(message, exception);
            }

            throw new PlayUnitTestActException(message, exception);
        }
        catch (Exception exception)
        {
            if (message != string.Empty)
            {
                Debug.WriteLine(message);

                throw new PlayUnitTestActException(message, exception);
            }

            throw new PlayUnitTestActException(message, exception);
        }
    }

    [SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>")]
    public static void Assertion(Action action, string message = "")
    {
        try
        {
            action.Invoke();
        }
        catch (XunitException exception)
        {
            if (message != string.Empty)
            {
                Debug.WriteLine(message);

                throw new PlayUnitTestAssertionException(message, exception);
            }

            throw new PlayUnitTestAssertionException(message, exception);
        }
        catch (Exception exception)
        {
            if (message != string.Empty)
            {
                Debug.WriteLine(message);

                throw new PlayUnitTestAssertionException(message, exception);
            }

            throw new PlayUnitTestAssertionException(message, exception);
        }
    }

    #endregion

    public static class Build
    {
        public new static class Equals
        {
            #region Instance Members

            public static string Message(string expected, string actual) => $"\n\n\t\texpected\t: {expected}; \n\t\tactual\t\t: {actual};";

            public static string Message(TagLengthValue expected, TagLengthValue actual) =>
                $"\n\n\t\texpected\t: {expected.EncodeTagLengthValue()}; \n\t\tactual\t\t: {actual.EncodeTagLengthValue()};";

            public static string Message(byte expected, byte actual) => $"\n\n\t\texpected\t: {expected}; \n\t\tactual\t\t: {actual};";
            public static string Message(int expected, int actual) => $"\n\n\t\texpected\t: {expected}; \n\t\tactual\t\t: {actual};";
            public static string Message(ushort expected, ushort actual) => $"\n\n\t\texpected\t: {expected}; \n\t\tactual\t\t: {actual};";
            public static string Message(uint expected, uint actual) => $"\n\n\t\texpected\t: {expected}; \n\t\tactual\t\t: {actual};";
            public static string Message(ulong expected, ulong actual) => $"\n\n\t\texpected\t: {expected}; \n\t\tactual\t\t: {actual};";

            public static string Message(byte[] expected, byte[] actual) =>
                $"\n\n\t\texpected\t: {BitConverter.ToString(expected).Replace("-", "")}; \n\t\tactual\t\t: {BitConverter.ToString(actual).Replace("-", "")};";

            public static string Message(Span<byte> expected, Span<byte> actual) =>
                "\n\n\t\texpected\t: {BitConverter.ToString(expected.ToArray())}; \n\t\tactual\t: {BitConverter.ToString(expected.ToArray())};";

            #endregion
        }
    }
}