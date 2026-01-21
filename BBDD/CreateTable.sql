CREATE TABLE Usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100),
    email VARCHAR(150) UNIQUE NOT NULL,
    contraseña VARCHAR(255) NOT NULL,
);

CREATE TABLE Capsula (
    id_capsula INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(150),
    descripcion TEXT,
    fecha_creacion DATE,
    fecha_apertura DATE,
    estado VARCHAR(20),
    visibilidad VARCHAR(20)
);

CREATE TABLE Usuario_Capsula (
    id_usuario_capsula INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT,
    id_capsula INT,
    rol VARCHAR(30),
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_capsula) REFERENCES Capsula(id_capsula)
);

CREATE TABLE Contenido (
    id_contenido INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(30),
    contenido TEXT,
    fecha_subida DATE,
    id_capsula INT,
    FOREIGN KEY (id_capsula) REFERENCES Capsula(id_capsula)
);

CREATE TABLE Comentario (
    id_comentario INT AUTO_INCREMENT PRIMARY KEY,
    texto TEXT,
    fecha_comentario DATE,
    id_usuario INT,
    id_capsula INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_capsula) REFERENCES Capsula(id_capsula)
);

CREATE TABLE Notificacion (
    id_notificacion INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(50),
    mensaje TEXT,
    fecha_creacion DATE,
    leida BOOLEAN,
    id_usuario INT,
    id_capsula INT NULL,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_capsula) REFERENCES Capsula(id_capsula)
);

CREATE TABLE Amistad (
    id_amistad INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario1 INT,
    id_usuario2 INT,
    estado VARCHAR(20),
    FOREIGN KEY (id_usuario1) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_usuario2) REFERENCES Usuario(id_usuario)
);