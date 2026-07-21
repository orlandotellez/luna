# 10. Invitación de Acompañante

**Descripción**: Una usuaria invita a un familiar como acompañante para que reciba contenido de apoyo y vea su calendario compartido.

**Actores**: Usuaria, Familiar, Sistema

**Tablas involucradas**: `family_members`, `users`, `family_messages`

```mermaid
sequenceDiagram
    actor U as Usuaria
    participant F as Frontend
    participant B as Backend (API)
    participant DB as PostgreSQL

    U->>F: Abre app → Acompañamiento → "Invitar acompañante"
    U->>F: Ingresa nombre y correo de su pareja
    U->>F: Selecciona parentesco: "Pareja"
    U->>F: Configura permisos: ✓ Calendario ✓ Alertas ✓ Mensajes
    F->>B: POST /family/invite { name, email, relationship, permissions }

    B->>B: Genera invitation_token
    B->>DB: INSERT INTO family_members (user_id, name, email, relationship, invitation_token, status='PENDING')
    B->>B: Envía email de invitación con link de registro
    B-->>F: 200

    Note over F: Familiar recibe el email
    actor A as Familiar (Pareja)
    A->>A: Abre link de invitación
    alt Familiar ya tiene cuenta LUNA
        A->>F: Inicia sesión
        F->>B: POST /family/accept-invitation { token }
        B->>DB: UPDATE family_members SET status='ACCEPTED', family_user_id=?
    else Familiar no tiene cuenta
        A->>F: Se registra con perfil "FAMILIAR"
        B->>DB: INSERT INTO users (name, email, role='FAMILIAR')
        B->>DB: UPDATE family_members SET status='ACCEPTED', family_user_id=?
    end

    B-->>A: Redirige al dashboard de acompañante
    A->>A: Ve calendario compartido + contenido de apoyo
```
