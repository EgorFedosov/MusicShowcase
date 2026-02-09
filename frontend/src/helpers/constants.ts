export const DEFAULT_PAGE_SIZE = 20;
export const DEFAULT_LIKES = 5;
export const DEFAULT_REGION = "en_US";
export const DEFAULT_SEED = 7777;

export const API_HOST = import.meta.env.VITE_API_URL;
export const API_BASE_PATH = "/api";

export const ENDPOINTS = {
  SONGS: "/songs",
  EXPORT: "/songs/export",
};

export const EXPORT_FILENAME = "songs.zip";

export const DETAIL_IMAGE_SIZE = 240;
export const PLACEHOLDER_IMAGE = `https://placehold.co/${DETAIL_IMAGE_SIZE}x${DETAIL_IMAGE_SIZE}?text=No+Image`;

export const GALLERY_GRID = {
  xs: 24,
  sm: 12,
  md: 8,
  lg: 6,
  xl: 6,
};

export const GALLERY_GUTTER: [number, number] = [16, 16];

export const TOTAL_SONGS_COUNT = 10000;

export const MAX_SEED_VALUE = 10000000;

export const LIKES_RANGE = {
  MIN: 0,
  MAX: 10,
  STEP: 0.1,
};

export const REGIONS = [
  { value: "en_US", label: "English (US)" },
  { value: "fr", label: "French (FR)" },
  { value: "it", label: "Italian (IT)" },
];

export const GENRE_COLORS: Record<string, string> = {
  Rock: "#f5222d",
  Pop: "#eb2f96",
  "Hip Hop": "#fa541c",
  Rap: "#fa8c16",
  Jazz: "#722ed1",
  Electronic: "#2f54eb",
  Metal: "#434343",
  Classical: "#faad14",
  Folk: "#5b7e00",
  Country: "#52c41a",
  Latin: "#13c2c2",
  Reggae: "#389e0d",
  Blues: "#1890ff",
  Soul: "#9254de",
  World: "#08979c",
  "Stage And Screen": "#0050b3",
  "Non Music": "#bfbfbf",
};