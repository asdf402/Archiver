## Archiver

# the programm sequence
Application -> Is the main orchestrater of the whole system. It instanziates nessesary objects.
First the Parser class is called. It creates and returns an object containing the inputed user information.
Second the ValidatorService is called. It creates instanzes of the different validator classes and then calls their validation functions.
Third a factory produces the command.
fourth the command is executed.