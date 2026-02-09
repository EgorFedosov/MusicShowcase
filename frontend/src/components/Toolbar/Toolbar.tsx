import {
  Flex,
  Select,
  InputNumber,
  Slider,
  Button,
  Tooltip,
  Segmented,
  Card,
} from "antd";
import {
  ReloadOutlined,
  DownloadOutlined,
  AppstoreOutlined,
  BarsOutlined,
  GlobalOutlined,
  NumberOutlined,
  LikeOutlined,
} from "@ant-design/icons";
import {
  DEFAULT_SEED,
  REGIONS,
  MAX_SEED_VALUE,
  LIKES_RANGE,
} from "../../helpers/constants";
import type { IGenerationParams } from "../../types";
import s from "./Toolbar.module.css";

interface ToolbarProps {
  params: IGenerationParams;
  view: "table" | "gallery";
  onParamChange: (params: Partial<IGenerationParams>) => void;
  onViewChange: (view: "table" | "gallery") => void;
  onExport: () => void;
  isExporting: boolean;
}

export const Toolbar = ({
  params,
  view,
  onParamChange,
  onViewChange,
  onExport,
  isExporting,
}: ToolbarProps) => {
  const handleRandomSeed = () => {
    const newSeed = Math.floor(Math.random() * MAX_SEED_VALUE);
    onParamChange({ seed: newSeed });
  };

  return (
    <Card
      variant="borderless"
      classNames={{ body: s.cardBody }}
      className={s.card}
    >
      <Flex justify="space-between" align="end" wrap="wrap" gap="middle">
        <Flex gap="large" align="end" wrap="wrap" style={{ flex: 1 }}>
          <div className={s.regionWrapper}>
            <span className={s.label}>
              <GlobalOutlined className={s.icon} /> Region
            </span>
            <Select
              value={params.region}
              onChange={(val) => onParamChange({ region: val })}
              options={REGIONS}
              className={s.fullWidth}
              size="large"
            />
          </div>

          <div className={s.seedWrapper}>
            <span className={s.label}>
              <NumberOutlined className={s.icon} /> Seed
            </span>
            <Flex>
              <InputNumber
                className={s.seedInput}
                value={params.seed}
                onChange={(val) => onParamChange({ seed: val || DEFAULT_SEED })}
                controls={false}
                size="large"
              />
              <Tooltip title="Randomize">
                <Button
                  size="large"
                  icon={<ReloadOutlined />}
                  onClick={handleRandomSeed}
                  className={s.seedButton}
                />
              </Tooltip>
            </Flex>
          </div>

          <div className={s.dividerLarge} />

          <div className={s.likesWrapper}>
            <Flex justify="space-between" align="baseline">
              <span className={s.label}>
                <LikeOutlined className={s.icon} /> Avg. Likes
              </span>
            </Flex>

            <Flex gap="middle" align="center">
              <Slider
                min={LIKES_RANGE.MIN}
                max={LIKES_RANGE.MAX}
                step={LIKES_RANGE.STEP}
                value={params.likesAverage}
                onChange={(val) => onParamChange({ likesAverage: val })}
                className={s.sliderWrapper}
                tooltip={{ formatter: (val) => `${val} likes` }}
                styles={{
                  track: { backgroundColor: "#1890ff", height: 6 },
                  rail: { backgroundColor: "#d9d9d9", height: 6 },
                }}
              />
              <InputNumber
                min={LIKES_RANGE.MIN}
                max={LIKES_RANGE.MAX}
                step={LIKES_RANGE.STEP}
                className={s.likesInput}
                value={params.likesAverage}
                onChange={(val) => onParamChange({ likesAverage: val || 0 })}
                controls={false}
                size="large"
              />
            </Flex>
          </div>
        </Flex>

        <Flex gap="small" align="end" className={s.actionsWrapper}>
          <Button
            type="primary"
            icon={<DownloadOutlined />}
            onClick={onExport}
            size="large"
            className={s.exportButton}
            loading={isExporting}
          >
            {isExporting ? "Exporting..." : "Export"}
          </Button>

          <div className={s.divider} />

          <Segmented
            value={view}
            onChange={onViewChange}
            size="large"
            options={[
              { value: "table", icon: <BarsOutlined /> },
              { value: "gallery", icon: <AppstoreOutlined /> },
            ]}
          />
        </Flex>
      </Flex>
    </Card>
  );
};