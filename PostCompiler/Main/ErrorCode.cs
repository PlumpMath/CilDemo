namespace PostCompiler.Main
{
    enum ErrorCode
    {
        Success = 0,
        WrongArguments = 1,
        FileDoesNotExists = 2,
        FileReadError = 3,
        ModuleReadError = 4, 
        ModuleWriteError = 5,
        FileWriteError = 6
    }
}
