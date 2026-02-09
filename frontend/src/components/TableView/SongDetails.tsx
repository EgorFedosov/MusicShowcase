import { Flex, Typography, Image, Card, Button, Tag } from "antd";
import { DownloadOutlined } from "@ant-design/icons";
import {
  API_HOST,
  PLACEHOLDER_IMAGE,
  DETAIL_IMAGE_SIZE,
  GENRE_COLORS,
} from "../../helpers/constants";
import type { ISong } from "../../types";
import s from "./SongDetails.module.css";

interface SongDetailsProps {
  song: ISong;
}

const { Title, Text, Paragraph } = Typography;

export const SongDetails = ({ song }: SongDetailsProps) => {
  return (
    <Card
      variant="borderless"
      className={s.card}
      classNames={{ body: s.cardBody }}
    >
      <Flex align="flex-start" wrap="wrap" className={s.mainFlex}>
        <div className={s.imageWrapper}>
          <Image
            width={DETAIL_IMAGE_SIZE}
            height={DETAIL_IMAGE_SIZE}
            src={`${API_HOST}${song.coverUrl}`}
            alt={song.title}
            className={s.coverImage}
            fallback={PLACEHOLDER_IMAGE}
          />
        </div>

        <Flex vertical className={s.contentWrapper} justify="space-between">
          <div>
            <Flex align="center" gap="small">
              <Title level={2} className={s.title}>
                {song.title}
              </Title>
              <Button
                type="text"
                icon={<DownloadOutlined />}
                href={`${API_HOST}${song.audioUrl}`}
                download
              />
            </Flex>

            <Text className={s.artistText}>
              by{" "}
              <Text strong className={s.artistName}>
                {song.artist}
              </Text>
            </Text>

            <TagList song={song} />
          </div>

          <div className={s.audioContainer}>
            <audio
              controls
              src={`${API_HOST}${song.audioUrl}`}
              className={s.audioPlayer}
            />
          </div>

          <div className={s.reviewBox}>
            <Text type="secondary" className={s.reviewLabel}>
              CRITIC REVIEW
            </Text>
            <Paragraph className={s.reviewText}>"{song.review}"</Paragraph>
          </div>
        </Flex>
      </Flex>
    </Card>
  );
};

const TagList = ({ song }: { song: ISong }) => (
  <Flex className={s.tagList}>
    <Tag color={GENRE_COLORS[song.genre] || "default"} className={s.genreTag}>
      {song.genre}
    </Tag>
    <span className={s.albumTag}>{song.album}</span>
  </Flex>
);