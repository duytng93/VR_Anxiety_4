namespace Enums {
    /// <summary>
    /// Enums representing the available languages, scenarios, and child avatars.
    /// </summary>
    public enum Languages 
    { 
        English,
        Spanish
    }
    public enum Scenarios 
    { 
        Home,
        School,
        Doctor
    }
    public enum ChildAvatars 
    { 
        Hispanic,
        Black,
        White,
        Asian
    }
    public enum SceneNames
    {
        GlobalScene,
        ChildScene, // TODO: Remove?
        StartScene,
        EndSceneWin,
        EndSceneLose,
        EndSceneWinSpanish,
        EndSceneLoseSpanish,
        TutorialScene,
        TutorialSceneSpanish,
        HomeScene,
        SchoolScene,
        DoctorScene
    }
}