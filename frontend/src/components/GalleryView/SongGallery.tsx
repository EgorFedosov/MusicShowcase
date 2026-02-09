import InfiniteScroll from "react-infinite-scroll-component";
import { Row, Col, Spin, Empty } from "antd";
import { SongCard } from "./SongCard";
import { GALLERY_GRID, GALLERY_GUTTER } from "../../helpers/constants";
import type { ISong } from "../../types";
import s from "./SongGallery.module.css";

interface SongGalleryProps {
  songs: ISong[];
  loadMore: () => void;
  hasMore: boolean;
}

export const SongGallery = ({ songs, loadMore, hasMore }: SongGalleryProps) => {
  if (songs.length === 0) {
    return (
      <div className={s.emptyContainer}>
        <Empty description="No songs loaded" />
      </div>
    );
  }

  return (
    <InfiniteScroll
      dataLength={songs.length}
      next={loadMore}
      hasMore={hasMore}
      loader={
        <div className={s.loaderContainer}>
          <Spin size="large" />
        </div>
      }
      endMessage={<div className={s.endMessage}>You have seen it all!</div>}
      className={s.scrollWrapper}
    >
      <Row gutter={GALLERY_GUTTER}>
        {songs.map((song) => (
          <Col key={song.index} {...GALLERY_GRID}>
            <SongCard song={song} />
          </Col>
        ))}
      </Row>
    </InfiniteScroll>
  );
};