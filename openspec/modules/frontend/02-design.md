# Design Identity

Identidad visual de LUNA.

---

## Philosophy

**Cálido, femenino, seguro, accesible.** Inspirado en FLO, Clue, Ovia, y la estética de aplicaciones de bienestar femenino.

Esta es una aplicación **PARA mujeres**, diseñada con empatía, colores cálidos y tipografía amigable. No es clínica ni fría. **Acogedora, profesional, empoderante.**

---

## Color Palette

### Backgrounds
```css
--bg-base:           #FFF5F5   /* fondo general - blanco rosado */
--bg-surface:        #FFFAFA   /* superficies, tarjetas */
--bg-card:           #FFFFFF   /* cards, inputs */
--bg-elevated:       #F8F0F5   /* dropdowns, tooltips, modales */
--bg-accent-light:   #F3E8F0   /* hover, selecciones */
```

### Text
```css
--text-primary:      #2D1B2E   /* casi negro con tono ciruela */
--text-secondary:    #6B5068   /* gris ciruela */
--text-muted:        #9B7F98   /* gris claro */
--text-on-primary:   #FFFFFF   /* texto sobre fondo primario */
--text-accent:       #D4A5B6   /* texto decorativo */
```

### Primary Colors
```css
--primary:           #D4A5B6   /* rosa empoderamiento */
--primary-dark:      #B88298   /* hover, active */
--primary-light:     #E8C8D4   /* background badges, pills */
```

### Secondary Colors
```css
--secondary:         #B8A9C9   /* lavanda */
--secondary-dark:    #9A8BB0   /* hover */
--secondary-light:   #D8CFE8   /* backgrounds */
```

### Accent / Safe Colors
```css
--accent-coral:      #E8A598   /* coral - alertas, destacados */
--accent-teal:       #88BDB0   /* teal - éxito, progreso, salud */
--accent-gold:       #E8C87A   /* dorado - logros, insignias */
```

### Semantic
```css
--success:           #5CB87A   /* verde salud */
--success-bg:        #E8F5E9   /* background éxito */
--warning:           #E8A020   /* amarillo */
--warning-bg:        #FFF3E0   /* background advertencia */
--danger:            #D4606A   /* rojo suave */
--danger-bg:         #FFEBEE   /* background error */
--info:              #7AB8D4   /* azul informativo */
--info-bg:           #E3F2FD   /* background info */
```

### Borders
```css
--border:            #E8DDE6   /* bordes suaves */
--border-strong:     #D4C5D0   /* bordes más marcados */
```

---

## Typography

### Fonts
- **Headings**: "Playfair Display" — weight 700/600 (serif elegante)
- **Body**: "Inter" — weight 400/500 (sans-serif legible)
- **Lenguas Originarias**: "Noto Sans" (soporte glifos extendidos)
- **Números/Datos**: "Inter" — weight 500/600 (dashboard)

### Font Sizes
```css
--text-xs:   12px    /* etiquetas pequeñas */
--text-sm:   14px    /* body pequeño, metadata */
--text-base: 16px    /* body principal */
--text-lg:   18px    /* subtítulos */
--text-xl:   22px    /* títulos sección */
--text-2xl:  28px    /* títulos pantalla */
--text-3xl:  36px    /* hero */
```

### Import
```typescript
// En app entry
import { PlayfairDisplay_700Bold, PlayfairDisplay_600SemiBold } from '@expo-google-fonts/playfair-display';
import { Inter_400Regular, Inter_500Medium, Inter_600SemiBold } from '@expo-google-fonts/inter';
import { NotoSans_400Regular } from '@expo-google-fonts/noto-sans';
```

---

## Spacing System

```css
--space-xs:   4px
--space-sm:   8px
--space-md:   16px
--space-lg:   24px
--space-xl:   32px
--space-2xl:  48px
--space-3xl:  64px
```

## UI Components Rules

| Property | Value |
|----------|-------|
| Cards radius | 16px |
| Inputs radius | 12px |
| Buttons radius | 24px (pill) |
| Badges radius | 20px (pill) |
| Modals radius | 24px (top) / 24px (full) |
| Bottom sheet radius | 24px top |
| Default border | 1px solid var(--border) |
| Focus ring | 2px solid var(--primary) + offset |
| Shadows | Soft (iOS) / Elevation (Android) |
| Hover animation | 150ms ease (donde aplique) |
| Screen transitions | iOS push / Android fade |
| Touch feedback | Opacity: 0.8 |

---

## Component Kit

### Buttons

| Variant | Style | Uso |
|---------|-------|-----|
| **Primary** | `bg: primary, text: white, pill` | Acción principal |
| **Secondary** | `bg: secondary, text: white, pill` | Acción secundaria |
| **Outline** | `border: primary, text: primary` | Acción alternativa |
| **Ghost** | `text: primary, no bg` | Links, acciones ligeras |
| **Icon** | Circular, 48x48 | FAB, icon buttons |
| **Disabled** | `opacity: 0.5` | Estado deshabilitado |

### Inputs

- Text input: fondo blanco, borde suave, label flotante o top
- Search bar: icono + input redondeado
- Date picker: nativo de plataforma
- Dropdown: bottom sheet con lista
- Slider: para escalas de dolor (1-5), intensidad de síntomas

### Cards

- **Health Card**: icono + título + valor + unidad
- **Symptom Card**: icono + nombre + selector intensidad
- **Article Card**: thumbnail + título + descripción breve + tiempo de lectura
- **Calendar Day**: circulito con color según estado (período, fértil, ovulación, síntoma)
- **Professional Card**: foto + nombre + especialidad + rating + ubicación

### Navigation

- **Bottom Tabs**: 4-5 tabs con icono + label
  - Inicio | Ciclo/Etapa | Biblioteca | Comunidad | Perfil
- **Top Tabs**: para sub-secciones dentro de una pantalla
- **Drawer**: menú lateral con avatar + nombre + enlaces

---

## Ilustraciones e Iconografía

- **Ilustraciones**: estilo cálido, flat illustration, tonos pastel
- **Iconos**: lucide-react-native, tamaño 24px estándar
- **Emojis**: uso moderado, solo en reacciones de comunidad
- **Avatares**: initials sobre fondo de color (iniciales + color asignado)

---

## Dark Mode (Futuro)

Cuando se implemente dark mode:
```css
--bg-base-dark:    #1A1220
--bg-surface-dark: #2A1D30
--bg-card-dark:    #352840
--text-primary-dark: #F5E8FA
```
