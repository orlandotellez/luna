export const colors = {
  // Brand
  primary: '#C8665C',
  primaryDark: '#A84C42',
  primaryLight: '#E07A70',
  secondary: '#FDF8F5',

  // Stage Colors
  menstruacion: '#E8846A',
  embarazo: '#E8A87C',
  menopausia: '#A88DB8',

  // Supporting
  salvia: '#8DB89A',
  salviaDark: '#5DAE7B',
  lavanda: '#C4A8D4',
  durazno: '#F4D4B8',

  // Neutrals
  bg: '#FDF8F5',
  bgAlt: '#FFF9F5',
  surface: '#FFFFFF',
  border: '#F5EDE8',
  textSecondary: '#8C7E76',
  textPrimary: '#3D322C',
  textDark: '#2B221D',
  placeholder: '#C4A8D4',

  // Semantic
  success: '#5DAE7B',
  error: '#D45656',
  warning: '#E4A454',

  // Stage backgrounds
  stageMenstruacionBg: '#FEF3EF',
  stageEmbarazoBg: '#FEF7F0',
  stageMenopausiaBg: '#F5F0F8',
  stageFamiliaBg: '#E8F0E8',

  // Misc
  chartBarDefault: '#E8D5D0',
  chartBarLight: '#E0BCB0',
  phoneBackground: '#F0EBE6',
  avatarBackground: '#E8D5C0',
} as const;

export type ColorKey = keyof typeof colors;
