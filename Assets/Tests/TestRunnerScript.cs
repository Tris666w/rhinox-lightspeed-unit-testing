// using UnityEditor.TestTools.TestRunner.Api;
// using UnityEngine;
//
// public class TestRunnerScript
// {
//     [RuntimeInitializeOnLoadMethod]
//     public static void RunTests()
//     {
//         var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
//         testRunnerApi.RegisterCallbacks(new TestCallbacks());
//
//         var filter = new Filter
//         {
//             testMode = TestMode.PlayMode
//         };
//
//         var settings = new ExecutionSettings(filter)
//         {
//             runSynchronously = true
//         };
//         Debug.Log("Starting tests");
//         testRunnerApi.Execute(settings);
//     }
//
//     private class TestCallbacks : IErrorCallbacks
//     {
//         public void OnError(string message)
//         {
//             Debug.Log(message);
//         }
//
//         public void RunFinished(ITestResultAdaptor result)
//         {
//             Debug.Log("Tests finished");
//         }
//
//         public void RunStarted(ITestAdaptor testsToRun)
//         {
//             Debug.Log("Tests started");
//         }
//
//         public void TestFinished(ITestResultAdaptor result)
//         {
//         }
//
//         public void TestStarted(ITestAdaptor test)
//         {
//         }
//     }
// }