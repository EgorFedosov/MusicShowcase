import axios from "axios";
import { saveAs } from "file-saver";
import type { IGenerationParams, ISongsResponse } from "../types";
import {
  DEFAULT_PAGE_SIZE,
  API_HOST,
  API_BASE_PATH,
  ENDPOINTS,
  EXPORT_FILENAME,
} from "../helpers/constants";

const apiClient = axios.create({
  baseURL: `${API_HOST}${API_BASE_PATH}`,
});

const getParamsWithDefaults = (params: IGenerationParams) => ({
  ...params,
  pageSize: params.pageSize || DEFAULT_PAGE_SIZE,
});

export const songsApi = {
  getSongs: async (params: IGenerationParams): Promise<ISongsResponse> => {
    const { data } = await apiClient.get<ISongsResponse>(ENDPOINTS.SONGS, {
      params: getParamsWithDefaults(params),
    });
    return data;
  },

  exportSongs: async (params: IGenerationParams) => {
    const { data } = await apiClient.get(ENDPOINTS.EXPORT, {
      params: getParamsWithDefaults(params),
      responseType: "blob",
    });
    saveAs(data, EXPORT_FILENAME);
  },
};