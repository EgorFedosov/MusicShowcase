import { useState, useCallback, useEffect } from "react";
import { message } from "antd";
import { songsApi } from "../api/songsApi";
import type { ISong, IGenerationParams } from "../types";

export const useSongs = (
  params: IGenerationParams,
  view: "table" | "gallery",
) => {
  const [songs, setSongs] = useState<ISong[]>([]);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);

  const fetchData = useCallback(
    async (isScrolling = false) => {
      setLoading(true);
      try {
        const response = await songsApi.getSongs(params);

        setHasMore(response.songs.length > 0);

        setSongs((prev) =>
          isScrolling ? [...prev, ...response.songs] : response.songs,
        );
      } catch (error) {
        console.error(error);
        message.error("Failed to load songs");
      } finally {
        setLoading(false);
      }
    },
    [params],
  );

  useEffect(() => {
    const isScrolling = view === "gallery" && params.page > 1;
    fetchData(isScrolling);
  }, [fetchData, params.page, view]);

  return {
    songs,
    loading,
    hasMore,
    setSongs,
  };
};