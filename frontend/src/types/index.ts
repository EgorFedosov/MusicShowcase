export interface ISong {
  index: number;
  title: string;
  artist: string;
  album: string;
  genre: string;
  review: string;
  likes: number;
  coverUrl: string;
  audioUrl: string;
}

export interface ISongsResponse {
  songs: ISong[];
  page: number;
}

export interface IGenerationParams {
  region: string;
  seed: number;
  likesAverage: number;
  page: number;
  pageSize?: number;
}
