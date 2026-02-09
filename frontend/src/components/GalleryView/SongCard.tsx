import { Card } from "antd";
import { LikeFilled } from "@ant-design/icons";
import { API_HOST } from "../../helpers/constants";
import type { ISong } from "../../types";
import s from "./SongCard.module.css";

interface SongCardProps {
  song: ISong;
}

const { Meta } = Card;

export const SongCard = ({ song }: SongCardProps) => {
  return (
    <Card
      hoverable
      variant="borderless"
      className={s.card}
      cover={
        <div className={s.coverWrapper}>
          <img
            alt={song.title}
            src={`${API_HOST}${song.coverUrl}`}
            className={s.coverImage}
          />
          <div className={s.genreBadge}>{song.genre}</div>
        </div>
      }
      styles={{ body: { padding: 0 } }}
    >
      <div className={s.cardBody}>
        <div className={s.metaHeader}>
          <div className={s.metaContent}>
            <Meta
              title={<span className={s.songTitle}>{song.title}</span>}
              description={<span className={s.artistName}>{song.artist}</span>}
            />
          </div>
          <div className={s.likesContainer}>
            {song.likes} <LikeFilled />
          </div>
        </div>

        <div className={s.footerSection}>
          <span className={s.albumText}>{song.album}</span>
          <audio
            controls
            src={`${API_HOST}${song.audioUrl}`}
            className={s.audioPlayer}
          />
        </div>
      </div>
    </Card>
  );
};
