-- Удаление таблиц для редактирования без ошибок
DROP TABLE IF EXISTS Appointments, Results, Patients, Doctors;

-- Создание таблицы "Doctors" с данными о врачах
CREATE TABLE Doctors (
    doctors_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	doctors_last_name VARCHAR(50) NOT NULL, -- Фамилия 
    doctors_first_name VARCHAR(50) NOT NULL, -- Имя
    doctors_middle_name VARCHAR(50), -- Отчество
    doctors_photo VARCHAR(255), -- Путь к фото варча
    job_position VARCHAR(50) NOT NULL, -- Должность
    username VARCHAR(50) NOT NULL UNIQUE, -- Логин пользователя
    user_password VARCHAR(255) NOT NULL -- Пароль пользователя
);

-- Создание таблицы "Patients" с данными о пациентах
CREATE TABLE Patients (
    patients_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	patients_last_name VARCHAR(50) NOT NULL, -- Фамилия 
    patients_first_name VARCHAR(50) NOT NULL, -- Имя
    patients_middle_name VARCHAR(50), -- Отчество
    patients_photo VARCHAR(255), -- Путь к фото пациента
    passport_data VARCHAR(20) NOT NULL,
    age INTEGER NOT NULL,
    gender VARCHAR(7) NOT NULL CHECK (gender IN ('Мужской', 'Женский'))
);

-- Создание таблицы "Results" с результатами из нейросети
CREATE TABLE Results (
    results_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    skin_photo VARCHAR(255), -- Путь к фото с кожей
    diagnosis VARCHAR(50) NOT NULL,
    description TEXT NOT NULL
);

-- Создание таблицы "Appointments" с обращениями к врачу
CREATE TABLE Appointments (
    Appointments_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	doctor_comment TEXT, -- Поле для комментария доктора
    fk_doctor_id INTEGER REFERENCES Doctors(doctors_id) NOT NULL,
    fk_patient_id INTEGER REFERENCES Patients(patients_id) NOT NULL,
    fk_result_id INTEGER REFERENCES Results(results_id) NOT NULL
	
);

/*
БД специально денормализирована, чтобы было меньше таблиц 
Возможные таблицы, которые были упрощены: Должность, Пол, Паспортные данные
*/


-- Заполнение таблицы Врачи для проверки соединения в VS
/*INSERT INTO Doctors (doctors_last_name, doctors_first_name, doctors_middle_name, doctors_photo, job_position, username, user_password) 
VALUES ('Tester', 'Test', 'Testing', 'none', 'Врач', 'doctor1', '123321')*/

-- Выборка из Врачи
SELECT * FROM Patients