# 🚀 RLM SHOW - Quiz Matemático 3D com Arduino

![Badge de Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)
![Badge de Tecnologia](https://img.shields.io/badge/Motor-Unity-black)
![Badge de Hardware](https://img.shields.io/badge/Hardware-Arduino%20Uno-blue)

## 📝 Descrição do Projeto

O **RLM SHOW** é um projeto simples de jogo de **quiz matemático em ambiente 3D**, desenvolvido na **Unity**.

O principal diferencial deste projeto é a sua **integração física com um Arduino Uno**. O Arduino é utilizado para fornecer *feedback tátil* e visual ao jogador, como acender LEDs verdes ou vermelhos para respostas corretas/incorretas, ou até mesmo acionar botões físicos para a interação dentro do jogo. Isso transforma a experiência digital em algo mais interativo e imersivo.

---

### ✨ Funcionalidades

* **Quiz em 3D:** Ambiente imersivo para apresentação e resposta das questões.
* **Geração de Questões:** Criação dinâmica de problemas matemáticos básicos.
* **Comunicação Serial:** Conexão robusta entre a Unity (C#) e o Arduino via porta serial.
* **Feedback Físico:** Utilização do Arduino para controlar componentes externos (LEDs, buzzers, etc.) com base na lógica do jogo.

---

### 🛠️ Tecnologias e Requisitos

| Categoria | Tecnologia/Ferramenta | Versão Recomendada |
| :--- | :--- | :--- |
| **Motor de Jogo** | Unity Engine | 6000.0.59f2 |
| **Linguagem Principal** | C# | .NET 8.x |
| **Microcontrolador** | Arduino IDE e Linguagem (Sketch) | lts |
| **Hardware** | Arduino Uno | N/A |

---

### 💻 Configuração e Instalação

#### 1. Projeto Unity

1.  Clone este repositório para a sua máquina local.
2.  Abra o projeto no Unity Hub, garantindo que você está usando a versão correta do Editor.
3.  A cena principal deve estar localizada em `Assets/Scenes/SampleScene.unity`.

#### 2. Código Arduino (Sketch)

1.  Navegue até a pasta `Arduino/` e abra o arquivo `.ino` (o sketch).
2.  Carregue o código para o seu **Arduino Uno** usando a IDE do Arduino.
3.  **Verifique a Lógica de Pinos:** Certifique-se de que os pinos de I/O (para LEDs, botões, etc.) no código do Arduino coincidem com a sua montagem física.

#### 3. Conexão Serial

A parte crucial é a configuração da comunicação:

* **Porta Serial:** No script de conexão da Unity (geralmente em C#), ajuste a variável que define a porta serial (ex: `COM4`, `/dev/ttyACM0`). Esta deve ser a porta exata em que o seu Arduino está conectado.
* **Baud Rate:** Garanta que o *Baud Rate* (taxa de transmissão) configurado no código Arduino (ex: `Serial.begin(9600);`) **é exatamente o mesmo** que está configurado no script da Unity.

---

### 🎮 Como Utilizar

1.  **Conecte o Arduino** ao seu PC via USB e verifique se o sketch foi carregado com sucesso.
2.  Abra o projeto na Unity e pressione o botão **Play** no Editor.
3.  O jogo apresentará uma questão de matemática na tela.
4.  O jogador pode interagir com os objetos 3D no ambiente para selecionar a resposta ou, se configurado, usar botões físicos conectados ao Arduino para enviar a resposta.
5.  O feedback será dado na tela (visual) e por aúdio.

---

### 🤝 Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para:

1.  Abrir uma **Issue** para relatar bugs ou sugerir melhorias.
2.  Submeter um **Pull Request** com correções de código ou novas funcionalidades.

---
