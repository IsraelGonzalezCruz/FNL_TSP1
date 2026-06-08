# 🎓 GUÍA MAESTRA — Examen Final TSP (VR/AR) · Unity 6.3

Kit listo para **jalar lo que necesites** en una escena maestra y resolver rápido.
Todo lo pesado (SDKs, modelos) se instala/descarga aparte para que el repo siga ligero.

---

## 📁 Estructura del kit (`Assets/_Examen/`)

| Carpeta | Qué hay |
|---|---|
| `Scripts/` | Scripts que **YA compilan** (no necesitan SDK) |
| `Plantillas/` | Scripts `.cs.txt` que se **activan** (renombrar a `.cs`) al instalar su SDK |
| `ImageTargets/` | `target_examen.jpg` listo para subir a Vuforia |
| `Modelos/` | Aquí pones tu modelo animado de Mixamo |
| `_LEEME/` | Esta guía |

---

## 🗺️ Mapa: práctica → archivo del kit

| Práctica | Script | ¿Necesita SDK? |
|---|---|---|
| P1 Concurrencia (demo 4 métodos) | `Scripts/Concurrencia.cs` | No ✅ |
| P1 Hilo secuencial vs concurrente | `Scripts/Flight.cs` / `FlightThread.cs` | No ✅ |
| P1 Sincronización de archivo (lock) | `Scripts/FlightThreadSync.cs` | No ✅ |
| VR locomoción (Cardboard) | `Scripts/VRWalk.cs` | No ✅ |
| P5 Servicio web (REST) | `Scripts/WebApiTest.cs` | No ✅ |
| P4 Serial / ESP32 | `Plantillas/SerialController.cs.txt` | Api .NET Framework |
| P3 Firebase Realtime DB | `Plantillas/BD.cs.txt` | Firebase SDK |
| P5 Web → Firestore | `Plantillas/FirestoreInitialize.cs.txt` | Firebase SDK |
| P2/P5 AR (Vuforia + Firestore) | `Plantillas/CardScript.cs.txt` | Vuforia + Firebase |
| AR Piano (toque/clic) | `Plantillas/PianoAR.cs.txt` | (Input clásico) |

---

## 🎬 ESCENA MAESTRA (activar/desactivar)

Idea: una sola escena con un **GameObject padre por práctica**, **desactivado**, y dentro lo que esa práctica necesita. En el examen solo **activas el que toque**.

Crea esta jerarquía (Hierarchy):
```
=== MASTER ===
  [P1_Concurrencia]   (desactivado)  -> Cubo + 4 esferas + objeto con Concurrencia.cs
  [P1_Flight]         (desactivado)  -> "Player" con Flight.cs o FlightThread.cs + cámara
  [VR_Cardboard]      (desactivado)  -> XR Origin / cámara VR + piso + VRWalk.cs
  [P5_WebAPI]         (desactivado)  -> objeto con WebApiTest.cs + (opcional) texto TMP
  [P4_Serial]         (desactivado)  -> "Player" + obstáculo trigger + SerialController.cs
  [P3_Firebase]       (desactivado)  -> Canvas (2 InputField, Toggle, 2 botones) + BD.cs
  [AR_Vuforia]        (desactivado)  -> ARCamera + ImageTarget + modelo/CardScript
```
**Cómo activar/desactivar:** selecciona el padre en *Hierarchy* → en el *Inspector*, arriba a la izquierda, marca/desmarca la **casilla junto al nombre** (eso es `GameObject.SetActive`).

> 💡 Puedes pedirle al **Agente de Unity** (Ctrl+Alt+A, modo Agent): *"Crea estos GameObjects vacíos como contenedores, desactivados, y nómbralos así: ..."* y que te arme el esqueleto en segundos.

---

## ✅ CHECKLIST DE ESTA NOCHE (lo que requiere cuenta/descarga)

### 1) Firebase (P3 y P5) — crea la base "por si las dudas"
1. Entra a **https://console.firebase.google.com** → *Add project* → nómbralo (ej. `examen-tsp`).
2. Dentro del proyecto:
   - **Build → Realtime Database → Create Database** → modo *test* (P3).
   - **Build → Firestore Database → Create Database** → modo *test* (P5).
3. Registra una app: ícono **Unity** (o Android) → *Bundle ID* ej. `com.tuNombre.ExamenTSP`.
4. Descarga **`google-services.json`**.
   - **Dónde va:** cópialo a `Assets/` (raíz). *(En P3 lo pusieron en `Assets/StreamingAssets/`; el SDK lo encuentra en cualquiera de las dos.)*
   - ⚠️ **NO lo subas a GitHub público** (ya está en el `.gitignore`).

### 2) Vuforia (P2/P5 — AR)
1. Crea cuenta en **https://developer.vuforia.com** → *Develop → License Manager → Get Basic (gratis)* → copia la **License Key**.
2. *Develop → Target Manager → Add Database* (tipo *Device*) → *Add Target* → sube **`Assets/_Examen/ImageTargets/target_examen.jpg`** (ancho ej. 1) → revisa que tenga ⭐⭐⭐⭐⭐.
3. *Download Database (All)* → formato **Unity Editor** → te da un `.unitypackage`.
   - **Dónde va:** lo importas en el examen con *Assets → Import Package → Custom Package*.

### 3) Modelo animado (para el Image Target)
1. Entra a **https://www.mixamo.com** (cuenta Adobe gratis).
2. Elige un personaje + una animación (ej. *Walking*) → **Download** → formato **FBX for Unity**, *With Skin*.
3. **Dónde va:** guárdalo en `Assets/_Examen/Modelos/`. En la escena AR se pone como **hijo del ImageTarget**.

---

## 🔗 LINKS DE SDK y DÓNDE SE AGREGAN

| SDK | Link de descarga | Cómo se agrega en Unity |
|---|---|---|
| **Firebase Unity SDK** | https://firebase.google.com/download/unity | Descomprime → `Assets → Import Package → Custom Package` → `FirebaseDatabase.unitypackage` (P3) y/o `FirebaseFirestore.unitypackage` (P5). Conserva `ExternalDependencyManager`. |
| **Vuforia Engine** | https://developer.vuforia.com/downloads/sdk | Importa `add-vuforia-engine-*.unitypackage` (doble clic o Custom Package). Luego `Window → Vuforia Configuration` → pega la **License Key**. |
| **Mixamo (modelos)** | https://www.mixamo.com | Descarga FBX → arrástralo a `Assets/_Examen/Modelos/`. |

> Estos SDK pesan cientos de MB: **se importan en el momento, NO se suben a GitHub** (el `.gitignore` ya excluye `Assets/Firebase`, `Assets/Vuforia`, etc.).

---

## 🌐 API DE PRUEBA (servicio web P5)

- Endpoint REST público (Yu-Gi-Oh!):
  `https://db.ygoprodeck.com/api/v7/cardinfo.php?name=Dark Magician`
- Ya está cableado en `Scripts/WebApiTest.cs` (cambia `cardName` o pega otro endpoint en `url`).
- Alternativas si piden otra: `https://api.agify.io/?name=israel` · `https://catfact.ninja/fact`

---

## ⚙️ AJUSTES CLAVE de Player Settings (según la práctica)

- **Serial (P4):** `Edit → Project Settings → Player → Other Settings → Api Compatibility Level = .NET Framework`.
- **Input clásico (PianoAR / Input.GetAxis):** `Active Input Handling = Both`.
- **AR/VR build:** plataforma **Android** (`File → Build Profiles → Android → Switch Platform`).

---

## 🛡️ Recordatorio de seguridad
NO subas a GitHub público: `google-services.json`, la **License Key** de Vuforia, ni los SDK.
Esos se ponen **a mano en el salón**. El `.gitignore` ya los protege.
